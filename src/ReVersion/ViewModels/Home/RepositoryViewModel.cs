using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ReVersion.Models.Home;
using ReVersion.Services.Settings;
using ReVersion.Services.SvnClient;
using ReVersion.Services.SvnClient.Requests;
using ReVersion.Utilities.Helpers;

namespace ReVersion.ViewModels.Home
{
    internal class RepositoryViewModel : BaseViewModel<RepositoryModel>
    {
        public RepositoryViewModel()
        {
            CheckoutCommand = CommandFromFunction(c=> Checkout());
            BrowseCommand = CommandFromFunction(c=> Browse());
        }
        
        #region Commands
        public ICommand CheckoutCommand { get; set; }
        public ICommand BrowseCommand { get; set; }
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

        #endregion

        private async void checkout(CheckoutRepositoryRequest request)
        {
            var svnClientService = new SvnClientService();

            Model.CheckoutEnabled = false;

            var result = await Task.Run(() => svnClientService.CheckoutRepository(request));

            Model.CheckoutEnabled = true;

            if (result)
            {
                Model.CheckedOut = true;
            }
        }
    }
}
