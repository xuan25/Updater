using System;
using System.Collections.Generic;
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
using Update;

namespace Demo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            VersionBox.Text = UpdateUtil.CurrentVersion;

            UpdateUtil.NewVersionFound += UpdateUtil_NewVersionFound;
            UpdateUtil.LatestVersion += UpdateUtil_LatestVersion;
            UpdateUtil.Quit += UpdateUtil_Quit;
            UpdateUtil.StartCleaning();
        }

        private void UpdateUtil_NewVersionFound(string description)
        {
            MessageBox.Show(description, "New version found");
        }

        private void UpdateUtil_LatestVersion()
        {
            MessageBox.Show("Latest version");
        }

        private void UpdateUtil_Quit()
        {
            this.Close();
        }

        private void CheckBtn_Click(object sender, RoutedEventArgs e)
        {
            UpdateUtil.StartCheckVersion();
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            UpdateUtil.RunUpdate();
        }
    }
}
