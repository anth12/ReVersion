using System;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Win32;
using ReVersion.Services.Analytics;
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
            var viewModel = new HomeViewModel();

            window.DataContext = viewModel;
            window.Show();
            
            Application.Current.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(AppDispatcherUnhandledException);

            if (HasRun() == false)
            {
                FirstRun();
            }
        }

        #region Event handling

        private void AppDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            ErrorLog.Log("Unhandled Exception", e.Exception);


#if DEBUG   // In debug mode do not custom-handle the exception, let Visual Studio handle it

            e.Handled = false;

#else

            ShowUnhandeledException(e);    

#endif     
        }

        private void ShowUnhandeledException(DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;

            string errorMessage =
                $@"An application error occurred.
Please check whether your data is correct and repeat the action. If this error occurs again there seems to be a more serious malfunction in the application, and you better close it and try again...
Error:
{e.Exception.Message +(e.Exception.InnerException != null? "\n" +e.Exception.InnerException.Message: null)}

Do you want to continue?
(if you click Yes the application will remain open, if you click No the application will close)";

            if (
                MessageBox.Show(errorMessage, "Application Error", MessageBoxButton.YesNoCancel, MessageBoxImage.Error) == MessageBoxResult.No)
            {
                Application.Current.Shutdown();
            }
        }

        #endregion

        #region Usage statistics

        private bool HasRun()
        {
            const string registryKey = @"HKEY_CURRENT_USER\ReVersion";
            const string registyValue = "FirstRun";
            if (Convert.ToInt32(Registry.GetValue(registryKey, registyValue, 0)) == 0)
            {
                //Change the value since the program has run once now
                Registry.SetValue(registryKey, registyValue, 1, RegistryValueKind.DWord);
                return true;
            }
            return false;
        }

        private void FirstRun()
        {
            AnalyticsService.Session.CreatePageViewRequest("First-Run", "First Run").Send();
        }

        #endregion

    }
}