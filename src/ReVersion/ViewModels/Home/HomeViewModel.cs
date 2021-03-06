﻿using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using ReVersion.Models.Home;
using ReVersion.Services.SvnServer;
using ReVersion.Utilities.Extensions;
using ReVersion.Utilities.Helpers;
using ReVersion.ViewModels.Settings;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ReVersion.ViewModels.Home
{
    internal class HomeViewModel : BaseViewModel<HomeModel>
    {
        public HomeViewModel()
        {
            Model = new HomeModel(this);

            _repositories = new ObservableCollection<RepositoryViewModel>();
            _settings = new SettingsViewModel(this);

            //Configure Commands
            OpenSettingsCommand = CommandFromFunction(x => OpenSettings());

            OpenSettingsCommand = CommandFromFunction(x => OpenSettings());
            ExitCommand = CommandFromFunction(x => Exit());

            ToggleBulkCheckoutCommand = CommandFromFunction(x => ToggleBulkCheckout());
            SvnRefreshCommand = CommandFromFunction(x => SvnRefresh());
            SvnUpdateCommand = CommandFromFunction(x => SvnUpdate());

            AboutCommand = CommandFromFunction(x => About());
            HelpCommand = CommandFromFunction(x => Help());

            BulkCheckoutCommand = CommandFromFunction(x => BulkCheckout());
            
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
            _filter = (model) => Model.Search.IsBlank()
                             || model.Model.Name.ToCamelCase().ToLower().Contains(Model.Search.ToCamelCase().ToLower());

        }
        
        #region Commands
        public ICommand OpenSettingsCommand { get; set; }

        public ICommand ExitCommand { get; set; }

        public ICommand ToggleBulkCheckoutCommand { get; set; }
        public ICommand SvnUpdateCommand { get; set; }
        public ICommand SvnRefreshCommand { get; set; }

        public ICommand AboutCommand { get; set; }
        public ICommand HelpCommand { get; set; }

        public ICommand BulkCheckoutCommand { get; set; }
        
        #region Menu events

        #region File

        private void OpenSettings()
        {
            Model.SettingsActive = !Model.SettingsActive;
        }

        private void Exit()
        {
            Application.Current.Shutdown();
        }

        #endregion

        #region Actions

        private void ToggleBulkCheckout()
        {
            foreach (var repository in Repositories)
            {
                repository.Model.BulkCheckoutActive = !repository.Model.BulkCheckoutActive;
            }

            CheckoutUpdated(null, null);
        }

        private void BulkCheckout()
        {
            foreach (var repository in Repositories)
            {
                if (repository.Model.IsChecked)
                {
                    repository.CheckoutCommand.Execute(null);
                }
            }
        }

        private void CheckoutUpdated(object sender, EventArgs e)
        {
            Model.BulkUpdateUpdated();
        }

        private void SvnUpdate()
        {
            //TODO
        }

        private void SvnRefresh()
        {
            LoadRepositories(true);
        }
        
        #endregion

        #region Help

        private void About()
        {
            var assembly = Assembly.GetExecutingAssembly();

            ((MetroWindow)Application.Current.MainWindow).ShowMessageAsync($"ReVersion Version: {assembly.GetName().Version}", "By Anthony Halliday");
        }
        

        private void Help()
        {
            Process.Start("https://github.com/anth12/ReVersion/wiki");
        }

        #endregion

        #endregion

        #endregion

        #region Business Logic

        private SettingsViewModel _settings;

        public SettingsViewModel Settings
        {
            get { return _settings; }
            set { SetField(ref _settings, value); }
        }

        private readonly Func<RepositoryViewModel, bool> _filter;

        private ObservableCollection<RepositoryViewModel> _repositories;

        public ObservableCollection<RepositoryViewModel> Repositories
        {
            get { return _repositories; }
            set { SetField(ref _repositories, value); }
        }


        internal async void LoadRepositories(bool forceReload = false)
        {
            //Application.Current.Dispatcher.Invoke(() =>
            //{
                Model.Loading = true;
            //});

            var subversionServerCollator = new SvnServerService();

            var result = await subversionServerCollator.ListRepositories(forceReload);
            

            Repositories.Clear();
            
            await Task.Delay(1000);

            var index = 0;

            foreach(var repo in result.Repositories)
            {
                if(index++ % 10 == 0)
                {
                    await Task.Delay(5);
                }

                Repositories.Add(new RepositoryViewModel(new RepositoryModel
                {
                    CheckedOut = repo.CheckedOut,
                    CheckoutEnabled = !repo.CheckedOut,
                    Name = repo.Name,
                    SvnServerId = repo.SvnServerId,
                    Url = repo.Url,
                    IsEnabled = true,
                    Window = Model.Window
                }));
            }


            Repositories.ToList().ForEach(r =>
            {
                r.Model.PropertyChanged += BulkUpdateChecked;
            });

            Application.Current.Dispatcher.Invoke(() =>
            {
                Model.Loading = false;
            });



            if (result.Messages.Any())
            {
                NotificationHelper.ShowResult(result);
            }
        }

        public void FilterUpdated()
        {
            cancelFiltering = true;
            Task.Run(() => Applyfilter());
        }

        private bool cancelFiltering;

        private void Applyfilter()
        {
            cancelFiltering = false;

            foreach (var repo in Repositories)
            {
                if (cancelFiltering)
                    break;

                var active = _filter.Invoke(repo);
                repo.Model.IsEnabled = active;
            }

            Model.FilterUpdated();
        }
        private void BulkUpdateChecked(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == nameof(RepositoryModel.IsChecked))
            {
                Model.BulkUpdateUpdated();
            }
        }


        #endregion
    }
}
