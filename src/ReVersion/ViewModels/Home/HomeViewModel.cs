using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using ReVersion.Models.Home;
using ReVersion.Services;
using ReVersion.Services.Settings;
using ReVersion.Services.SvnServer;
using ReVersion.Services.SvnServer.Response;
using ReVersion.Utilities.Helpers;
using ReVersion.Views;

namespace ReVersion.ViewModels.Home
{
    public class HomeViewModel : BaseViewModel<HomeModel>
    {
        public HomeViewModel()
        {

            //Configure Commands
            ImportSettingsCommand = CommandFromFunction(x => ImportSettings());
            ExportSettingsCommand = CommandFromFunction(x => ExportSettings());
            OpenSettingsCommand = CommandFromFunction(x => OpenSettings());

            OpenSettingsCommand = CommandFromFunction(x => OpenSettings());
            ExitCommand = CommandFromFunction(x => Exit());

            SvnRefreshCommand = CommandFromFunction(x => SvnRefresh());
            SvnUpdateCommand = CommandFromFunction(x => SvnUpdate());

            AboutCommand = CommandFromFunction(x => About());
            HelpCommand = CommandFromFunction(x => Help());

            //Model binding
            Model.Repositories.CollectionChanged += (sender, args) => { ApplyFilter(); };

            PropertyChanged += (sender, args) =>
            {
                if (!Model.Loading && args.PropertyName == nameof(Model.Search) ||
                    args.PropertyName == nameof(Model.Repositories))
                {
                    ApplyFilter();

                    OnPropertyChanged(nameof(Model.CountSummary));
                }
            };

            //Finally, load the data
            LoadRepositories();
        }
        
        #region Commands
        public ICommand ImportSettingsCommand { get; set; }
        public ICommand ExportSettingsCommand { get; set; }
        public ICommand OpenSettingsCommand { get; set; }

        public ICommand ExitCommand { get; set; }

        public ICommand SvnUpdateCommand { get; set; }
        public ICommand SvnRefreshCommand { get; set; }

        public ICommand AboutCommand { get; set; }
        public ICommand HelpCommand { get; set; }


        #region Menu events

        #region File

        private void ImportSettings()
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

        private void ExportSettings()
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

        private void OpenSettings()
        {
            var settings = new SettingsWindow();
            settings.Show();
        }

        private void Exit()
        {
            Application.Current.Shutdown();
        }

        #endregion

        #region Actions

        private void SvnUpdate()
        {
            //TODO
        }

        private void SvnRefresh()
        {
            //TODO
        }

        #endregion

        #region Help

        private void About()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            MessageBox.Show($"Version: {fvi.FileVersion}");
        }

        private void Help()
        {
            NotificationHelper.ShowResult(Result.Success("test message goes herre"));

            NotificationHelper.ShowResult(Result.Error("test message goes herre"));

            //Process.Start("http://github.com/anth12/ReVersion");
        }

        #endregion

        #endregion

        #endregion
        
        #region Business Logic

        private async void ApplyFilter()
        {
            //TODO make async
            var searchTerm = Model.Search.ToLower();

            //Apply the filtering
            var filteredRepositories = Model.Repositories
                .Where(repo => repo.Name.ToLower().Contains(searchTerm))
                .ToList();


            Model.FilteredRepositories.Clear();
            OnPropertyChanged(nameof(Model.CountSummary));
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
