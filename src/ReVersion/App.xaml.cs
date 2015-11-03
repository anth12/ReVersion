using System.Windows;
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
            var viewModel = new HomeViewModel();
            var window = new HomeWindow
            {
                DataContext = viewModel
            };

            window.Show();
        }
    }
}