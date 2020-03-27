using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Updater.Util;

namespace Updater
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public Color ThemeColor
        {
            get
            {
                return (Color)GetValue(ThemeColorProperty);
            }
            set
            {
                SetValue(ThemeColorProperty, value);
            }
        }
        public static readonly DependencyProperty ThemeColorProperty = DependencyProperty.Register("ThemeColor", typeof(Color), typeof(MainWindow), new FrameworkPropertyMetadata(Color.FromArgb(0xff, 0xf2, 0x5e, 0x8d)));

        public Color HoveredColor
        {
            get
            {
                return (Color)GetValue(HoveredColorProperty);
            }
            set
            {
                SetValue(HoveredColorProperty, value);
            }
        }
        public static readonly DependencyProperty HoveredColorProperty = DependencyProperty.Register("HoveredColor", typeof(Color), typeof(MainWindow), new FrameworkPropertyMetadata(Color.FromArgb(0x88, 0x79, 0x2e, 0x47)));

        public Color ProgressColor
        {
            get
            {
                return (Color)GetValue(ProgressColorProperty);
            }
            set
            {
                SetValue(ProgressColorProperty, value);
            }
        }
        public static readonly DependencyProperty ProgressColorProperty = DependencyProperty.Register("ProgressColor", typeof(Color), typeof(MainWindow), new FrameworkPropertyMetadata(Color.FromArgb(0xff, 0x00, 0xa1, 0xd6)));


        private Downloader UpdateDownloader { get; set; }
        private string Filepath { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string[] args = (string[])Application.Current.Properties["Args"];
            if (args.Length != 1)
            {
                Close();
                return;
            }
            Filepath = args[0];
            Update();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (UpdateDownloader != null && UpdateDownloader.IsRunning)
            {
                UpdateDownloader.CancelDownload();
            }
        }

        private void Update()
        {
            UpdateDownloader = new Downloader(Filepath);
            UpdateDownloader.ProgressUpdated += Downloader_ProgressUpdated;
            UpdateDownloader.Finished += Downloader_Finished;
            UpdateDownloader.StartDownloadLatest();
        }

        private void Downloader_Finished()
        {
            Process.Start(Filepath);
            Dispatcher.Invoke(new Action(() =>
            {
                this.Close();
            }));
        }

        private void Downloader_ProgressUpdated(Downloader.Status status, double progress, long bps)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                InfoBox.Foreground = new SolidColorBrush(Color.FromRgb(0x00, 0x00, 0x00));
                switch (status)
                {
                    case Downloader.Status.Downloading:
                        InfoBox.Text = string.Format("{0:0.0}%    {1}    下载中...", progress, FormatBps(bps));
                        break;
                    case Downloader.Status.Initializing:
                        InfoBox.Text = "正在初始化...";
                        break;
                    case Downloader.Status.Waiting:
                        InfoBox.Text = "等待主程序关闭...";
                        break;
                    case Downloader.Status.Finishing:
                        InfoBox.Text = "正在完成...";
                        break;
                    case Downloader.Status.Finished:
                        InfoBox.Text = "更新完成!!!";
                        break;
                }
                UpdateProgressBar.Value = progress;
            }));
        }

        private string FormatBps(long bps)
        {
            if (bps < 1024)
                return string.Format("{0:0.0} Byte/s", bps);
            else if (bps < 1024 * 1024)
                return string.Format("{0:0.0} KB/s", (double)bps / 1024);
            else
                return string.Format("{0:0.0} MB/s", (double)bps / (1024 * 1024));
        }

        private void HeaderGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void MinimizeBtn_Clicked(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseBtn_Clicked(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }

    public class SolidColorBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new SolidColorBrush((Color)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
