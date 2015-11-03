using System;
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
using ReVersion.Utilities.Extensions;
using ReVersion.Utilities.Helpers;
using ReVersion.Views;

namespace ReVersion.ViewModels.Home
{
    public class HomeViewModel : BaseViewModel<HomeModel>
    {
        public HomeViewModel()
        {
            Model = new HomeModel(this);

            repositories = new ObservableCollection<RepositoryViewModel>();

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
            Repositories.CollectionChanged += (sender, args) => { Model.FilterUpdated(); };

            Model.PropertyChanged += (sender, args) =>
            {
                if (!Model.Loading && args.PropertyName == nameof(Model.Search) ||
                    args.PropertyName == nameof(Repositories))
                {
                    FilterUpdated();
                }
            };


            //Configure the search filter
            Filter = (model) => Model.Search.IsBlank()
                             || model.Model.Name.ToLower().Contains(Model.Search.ToLower());

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

        private Func<RepositoryViewModel, bool> Filter;

        private ObservableCollection<RepositoryViewModel> repositories;

        public ObservableCollection<RepositoryViewModel> Repositories
        {
            get { return repositories; }
            set { SetField(ref repositories, value); }
        }


        private async void LoadRepositories(bool forceReload = false)
        {
            Model.Loading = true;

            var subversionServerCollator = new SvnServerService();

            var result = await subversionServerCollator.ListRepositories(forceReload);

            Repositories.Clear();
            result.Repositories.ForEach(repo => Repositories.Add(new RepositoryViewModel
            {
                Model = new RepositoryModel
                {
                    CheckedOut = repo.CheckedOut,
                    Name = repo.Name,
                    SvnServerId = repo.SvnServerId,
                    Url = repo.Url,
                    IsEnabled = true,
                    Visibility = Visibility.Visible
                }
            }));

            Model.Loading = false;

            if (result.Messages.Any())
            {
                NotificationHelper.ShowResult(result);
            }
        }

        public void FilterUpdated()
        {
            foreach (var repo in Repositories)
            {
                var active = Filter.Invoke(repo);
                repo.Model.IsEnabled = active;
                repo.Model.Visibility = active ? Visibility.Visible : Visibility.Collapsed;
            }

            Model.FilterUpdated();
        }


        #endregion
    }
}
