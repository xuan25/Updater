using JsonUtil;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Update
{
    class UpdateUtil
    {
        public delegate void NewVersionFoundHandler(string description);
        /// <summary>
        /// Occurs when a new version has been recieved.
        /// </summary>
        public static event NewVersionFoundHandler NewVersionFound;

        public delegate void LatestVersionHandler();
        /// <summary>
        /// Occurs when a current version is the latest version.
        /// </summary>
        public static event LatestVersionHandler LatestVersion;

        public delegate void QuitHandler();
        /// <summary>
        /// Occurs when an update has been started and request the main program to exit.
        /// </summary>
        public static event QuitHandler Quit;

        private const string UserAgent = "UpdaterDemo";
        private const string RepoName = "xuan525/Updater";
        private const string UpdaterUri = "Updater/updater.exe";
        private static string UpdaterPath => AppDomain.CurrentDomain.BaseDirectory + "updater.exe";
        public static string CurrentVersion => Application.Current.FindResource("Version").ToString();

        public static bool IsCleaning { get; private set; }
        public static FileStream CheckingFileStream { get; private set; }
        public static Thread CleaningThread { get; private set; }
        public static Thread CheckVersionThread { get; private set; }

        static UpdateUtil()
        {
            IsCleaning = false;
        }

        public static void StopAll()
        {
            StopCheckVersion();
            StopCleaning();
        }

        public static void StopCleaning()
        {
            if (CleaningThread != null)
                CleaningThread.Abort();
        }

        public static void StartCleaning()
        {
            if (File.Exists(UpdaterPath))
            {
                IsCleaning = true;
                CleaningThread = new Thread(delegate ()
                {
                    while (IsFileInUse(UpdaterPath))
                    {
                        Thread.Sleep(1000);
                    }
                    File.Delete(UpdaterPath);
                    IsCleaning = false;
                });
                CleaningThread.Start();
            }
        }

        public static void StopCheckVersion()
        {
            if (CheckVersionThread != null)
                CheckVersionThread.Abort();
            if (CheckingFileStream != null)
                CheckingFileStream.Close();
        }

        public static void StartCheckVersion()
        {
            CheckVersionThread = new Thread(delegate ()
            {
                if (!IsLatestVersion(out string description))
                {
                    NewVersionFound?.Invoke(description);
                }
                else
                {
                    LatestVersion?.Invoke();
                }
            });
            CheckVersionThread.Start();
        }

        public static bool IsFileInUse(string fileName)
        {
            bool inUse = true;
            try
            {
                CheckingFileStream = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                inUse = false;
            }
            catch
            {

            }
            finally
            {
                if (CheckingFileStream != null)
                    CheckingFileStream.Close();
            }
            return inUse;
        }

        public static bool IsLatestVersion(out string description)
        {
            description = null;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("https://api.github.com/repos/{0}/releases/latest", RepoName));
            request.Accept = "application/vnd.github.v3+json";
            request.UserAgent = UserAgent;
            try
            {
                string result;
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                    result = reader.ReadToEnd();

                Json.Value json = Json.Parser.Parse(result);
                string latestTag = json["tag_name"];
                if (CurrentVersion == latestTag)
                    return true;
                description = ((string)json["body"]).Replace("\\r", "\r").Replace("\\n", "\n");
                return false;
            }
            catch (Exception ex)
            {
                return true;
            }
        }

        public static void RunUpdate()
        {
            while (IsCleaning) ;
            try
            {
                using (Stream stream = Application.GetResourceStream(new Uri(UpdaterUri, UriKind.Relative)).Stream)
                {
                    using (FileStream fileStream = new FileStream(UpdaterPath, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        stream.CopyTo(fileStream);
                    }
                }
                Process.Start(UpdaterPath, string.Format("\"{0}\"", Process.GetCurrentProcess().MainModule.FileName));
            }
            catch (UnauthorizedAccessException)
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo(Process.GetCurrentProcess().MainModule.FileName, "-update")
                {
                    Verb = "runas"
                };
                Process.Start(processStartInfo);
            }
            Quit?.Invoke();
        }
    }
}
