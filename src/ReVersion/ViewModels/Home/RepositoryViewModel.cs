using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ReVersion.Models.Home;
using ReVersion.Services.Settings;
using ReVersion.Services.SvnClient;
using ReVersion.Services.SvnClient.Requests;
using ReVersion.Utilities.Helpers;
using SharpSvn;

namespace ReVersion.ViewModels.Home
{
    internal class RepositoryViewModel : BaseViewModel<RepositoryModel>
    {
        public RepositoryViewModel()
        {
            CheckoutCommand = CommandFromFunction(c=> Checkout());
            BrowseCommand = CommandFromFunction(c=> Browse());
            ViewLogCommand = CommandFromFunction(c=> ViewLog());
            RepoBrowserCommand = CommandFromFunction(c=> RepoBrowser());

            ButtonOptions = new ObservableCollection<RepositoryAction>
            {
                new RepositoryAction {Title = "Browse", Command = BrowseCommand},
                new RepositoryAction {Title = "Checkout", Command = CheckoutCommand},
                new RepositoryAction {Title = "View Log", Command = ViewLogCommand},
                new RepositoryAction {Title = "Repo Browser", Command = RepoBrowserCommand}
            };

        }


        private ObservableCollection<RepositoryAction> _buttonOptions;

        #region Commands
        public ICommand CheckoutCommand { get; set; }
        public ICommand BrowseCommand { get; set; }
        public ICommand ViewLogCommand { get; set; }
        public ICommand RepoBrowserCommand { get; set; }
        #endregion


        public ObservableCollection<RepositoryAction> ButtonOptions
        {
            get { return _buttonOptions; }
            set { SetField(ref _buttonOptions, value); }
        }

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
            var svnClientService = new SvnClientService();

            svnClientService.ClientProgress += (sender, status) =>
            {
                // var client = (SvnClient) sender; If required...
                Model.Progress = status.Progress;
            };

            Model.ShowProgress = true;
            Model.CheckoutEnabled = false;

#pragma warning disable 4014
            Task.Run(() => svnClientService.RepositorySize(new GetRepositorySizeRequest
#pragma warning restore 4014
            {
                SvnServerUrl = request.SvnServerUrl,
                RepositorySize = (size) => Model.RepositorySize = size
            }));


            var result = await Task.Run(() => svnClientService.CheckoutRepository(request));


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
