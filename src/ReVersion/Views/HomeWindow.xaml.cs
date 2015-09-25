using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using ReVersion.Helpers;
using ReVersion.Models;
using ReVersion.Services;
using ReVersion.Services.Settings;
using ReVersion.Services.Subversion;

namespace ReVersion.Views
{
    /// <summary>
    /// Interaction logic for HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : MetroWindow
    {
        protected HomeModel Model { get; set; } = new HomeModel();

        public HomeWindow()
        {
            InitializeComponent();
            ConfigureSearch();

            DataContext = Model;
        }

        #region Window Open/Close events

        private void HomeWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            LoadRepositories();
        }

        private void HomeWindow_OnClosed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        #endregion

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
                var result = SettingsService.Import(fileDialog.FileName);
                if (result != null)
                    NotificationHelper.ShowResult(result);
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
            //TODO
        }

        private void RefreshRepoList_OnClick(object sender, RoutedEventArgs e)
        {
            LoadRepositories();
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

            NotificationHelper.ShowResult(Result.Success("test message goes herre"));

            NotificationHelper.ShowResult(Result.Error("test message goes herre"));

            //Process.Start("http://github.com/anth12/ReVersion");
        }

        #endregion

        #endregion


        #region Private helper methods

        private void ConfigureSearch()
        {
            Model.Repositories.CollectionChanged += (sender, args) =>
            {
                ApplyFilder();
            };

            Model.PropertyChanged += (sender, args) =>
            {
                if(!Model.Loading && args.PropertyName == nameof(Model.Search) || args.PropertyName == nameof(Model.Repositories))
                {
                    ApplyFilder();
                    
                    Model.OnPropertyChanged(nameof(Model.CountSummary));
                }

            };
        }

        private async void ApplyFilder()
        {
            //TODO make async
            var searchTerm = Model.Search.ToLower();

            //Apply the filtering

            var filteredRepositories = Model.Repositories
                .Where(repo => repo.Name.ToLower().Contains(searchTerm))
                .ToList();

                
            Model.FilteredRepositories.Clear();
            filteredRepositories.ForEach(Model.FilteredRepositories.Add);

        }

        private async void LoadRepositories()
        {
            Model.Loading = true;

            var subversionServerCollator = new SubversionServerCollator();

            var result = await subversionServerCollator.ListRepositories();

            Model.Repositories.Clear();
            result.Repositories.ForEach(repo=> Model.Repositories.Add(repo));

            Model.Loading = false;

            if (result.Messages.Any())
            {
                //TODO

            }
        }

        #endregion

    }
}
