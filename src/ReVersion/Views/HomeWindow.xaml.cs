using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using ReVersion.Models;
using ReVersion.Services;
using ReVersion.Services.Settings;
using ReVersion.Services.SvnClient;
using ReVersion.Services.SvnServer;
using ReVersion.Utilities.Helpers;

namespace ReVersion.Views
{
    /// <summary>
    ///     Interaction logic for HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : MetroWindow
    {
        public HomeWindow()
        {
            InitializeComponent();
            ConfigureSearch();

            DataContext = Model;
        }

        protected HomeModel Model { get; set; } = new HomeModel();

        #region Window Open/Close events

        private void HomeWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            LoadRepositories();

            var svnClientService = new SvnClientService();
            //svnClientService.CheckoutRepository(new CheckoutRepositoryRequest
            //{
            //    ProjectName = "yotel_cms",
            //    SvnServerUrl = "http://10.0.0.18/svn"
            //});
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
            LoadRepositories(true);
        }

        #endregion

        #region Help

        private void About_OnClick(object sender, RoutedEventArgs e)
        {
            var assembly = Assembly.GetExecutingAssembly();
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
            Model.Repositories.CollectionChanged += (sender, args) => { ApplyFilter(); };

            Model.PropertyChanged += (sender, args) =>
            {
                if (!Model.Loading && args.PropertyName == nameof(Model.Search) ||
                    args.PropertyName == nameof(Model.Repositories))
                {
                    ApplyFilter();

                    Model.OnPropertyChanged(nameof(Model.CountSummary));
                }
            };
        }

        private async void ApplyFilter()
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

        private async void LoadRepositories(bool forceReload = false)
        {
            Model.Loading = true;

            var subversionServerCollator = new SvnServerService();

            var result = await subversionServerCollator.ListRepositories(forceReload);

            Model.Repositories.Clear();
            result.Repositories.ForEach(repo => Model.Repositories.Add(repo));

            Model.Loading = false;

            if (result.Messages.Any())
            {
                NotificationHelper.ShowResult(result);
            }
        }

        #endregion
    }
}