using System;
using System.Diagnostics;
using System.Windows;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using ReVersion.Services.Settings;

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
            var fileDialog = new OpenFileDialog
            {
                Filter = "Settings Files (.json)|*.json",
                FilterIndex = 1
            };

            var clicked = fileDialog.ShowDialog();

            if (clicked != null && clicked.Value)
            {
                SettingsService.Import(fileDialog.FileName);
            }
        }

        private void ExportSettings_OnClick(object sender, RoutedEventArgs e)
        {
            var fileDialog = new SaveFileDialog
            {
                Filter = "Settings Files (.json)|*.json"
            };

            var clicked = fileDialog.ShowDialog();

            if (clicked != null && clicked.Value)
            {
                SettingsService.Export(fileDialog.FileName);
            }
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

        #region Help
        
        private void About_OnClick(object sender, RoutedEventArgs e)
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            MessageBox.Show($"Version: {fvi.FileVersion}");
        }

        private void Help_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start("http://github.com/anth12/ReVersion");
        }

        #endregion

        #endregion

        private void HomeWindow_OnClosed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

    }
}
