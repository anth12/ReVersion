using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using ReVersion.Models.Home;
using ReVersion.Services.Settings;
using ReVersion.Services.SvnClient;
using ReVersion.Services.SvnClient.Requests;
using ReVersion.Utilities.Extensions;
using ReVersion.Utilities.Helpers;
using ReVersion.ViewModels.MarkDown;
using ReVersion.Views;
using SharpSvn;

namespace ReVersion.ViewModels.Home
{
    internal class RepositoryViewModel : BaseViewModel<RepositoryModel>
    {
        public RepositoryViewModel()
        {
            CheckoutCommand = CommandFromFunction(c=> Checkout());
            BrowseCommand = CommandFromFunction(c=> Browse());

            ReadMeCommand = CommandFromFunction(c=> ReadMe());
            ViewLogCommand = CommandFromFunction(c=> ViewLog());
            RepoBrowserCommand = CommandFromFunction(c=> RepoBrowser());

        }

        
        #region Commands
        public ICommand CheckoutCommand { get; set; }
        public ICommand BrowseCommand { get; set; }

        public ICommand ReadMeCommand { get; set; }
        public ICommand ViewLogCommand { get; set; }
        public ICommand RepoBrowserCommand { get; set; }
        #endregion
        
        #region Events
        private void Checkout()
        {
            var svnServer = SettingsService.Current.Servers.First(s => s.Id == Model.SvnServerId);

            checkout(new CheckoutRepositoryRequest
            {
                SvnUsername = svnServer.Username,
                SvnPassword = svnServer.RawPassword,
                ProjectName = Model.Name,
                SvnServerUrl = Model.Url
            });

        }

        private void Browse()
        {
            Process.Start(DirectoryHelper.GetRepositoryFolder(Model.Name));
        }


        private void ReadMe()
        {
            var svnServer = SettingsService.Current.Servers.First(s => s.Id == Model.SvnServerId);

            var svnClient = new SvnClientService();

            var readmeFile = svnClient.GetReadMeFile(new GetReadMeFileRepositoryRequest
            {
                SvnUsername = svnServer.Username,
                SvnPassword = svnServer.RawPassword,
                SvnServerUrl = Model.Url,
                ProjectName = Model.Name
            });

            if (readmeFile.IsNotBlank())
            {
                var window = new MarkDownWindow(
                    new MarkDownViewModel(readmeFile)
                    );

                window.Title = $"{Model.Name} - Read Me";
                window.Show();
            }
            else
            {
                var assembly = Assembly.GetExecutingAssembly();

                ((MetroWindow)Application.Current.MainWindow).ShowMessageAsync(
                    $"Unable to locate ReadMe file for {Model.Name}", "test"
                );

            }
        }

        private void ViewLog()
        {
            MessageBox.Show("Show log for: " + Model.Name);
        }

        private void RepoBrowser()
        {
            MessageBox.Show("Repo browser for: " + Model.Name);
        }

        #endregion

        private async void checkout(CheckoutRepositoryRequest request)
        {
            var svnClient = new SvnClientService();

            svnClient.ClientProgress += (sender, status) =>
            {
                // var client = (SvnClient) sender; If required...
                Model.Progress = status.Progress;
            };

            Model.ShowProgress = true;
            Model.CheckoutEnabled = false;

            svnClient.RepositorySize(new GetRepositorySizeRequest
            {
                SvnServerUrl = request.SvnServerUrl,
                RepositorySize = (size) => Model.RepositorySize = size
            });


            var result = await Task.Run(() => svnClient.CheckoutRepository(request));


            Model.Progress = Model.RepositorySize;
            Model.CheckoutEnabled = true;
            Model.ShowProgress = false;

            if (result)
            {
                Model.CheckedOut = true;
            }
        }
    }
}
