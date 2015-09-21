using System;
using System.Windows;
using MahApps.Metro.Controls;

namespace ReVersion.Views
{
    /// <summary>
    /// Interaction logic for HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : MetroWindow
    {
        public HomeWindow()
        {
            InitializeComponent();
        }

        #region Menu events

        #region File

        private void ImportSettings_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ExportSettings_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OpenSettings_OnClick(object sender, RoutedEventArgs e)
        {
            var settings = new SettingsWindow();
            settings.Show();
        }

        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        #endregion

        #region Actions

        private void SvnUpdate_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void RefreshRepoList_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        
        #endregion

        #endregion

        private void HomeWindow_OnClosed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
