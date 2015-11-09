﻿using System.Windows;
using ReVersion.Services.ErrorLogging;
using ReVersion.ViewModels.Home;
using ReVersion.Views;

namespace ReVersion
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            var window = new HomeWindow();
            var viewModel = new HomeViewModel(window);

            window.DataContext = viewModel;
            window.Show();

            Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
        }

        private void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            ErrorLog.Log("Unhandled Exception", e.Exception);
        }
    }
}