
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ReVersion.Models.Home;
using ReVersion.Services.Settings;
using ReVersion.Services.SvnClient;
using ReVersion.Services.SvnClient.Requests;

namespace ReVersion.ViewModels.Home
{
    public class RepositoryViewModel : BaseViewModel<RepositoryModel>
    {
        public RepositoryViewModel()
        {

            CheckoutCommand = CommandFromFunction(c=> Checkout());
        }
        
        #region Commands
        public ICommand CheckoutCommand { get; set; }
        #endregion

        #region Events
        private void Checkout()
        {
            var svnServer = SettingsService.Current.Servers.First(s => s.Id == Model.SvnServerId);

            checkout(new CheckoutRepositoryRequest
            {
                SvnUsername = svnServer.Username,
                SvnPassword = svnServer.GetPassword(),
                ProjectName = Model.Name,
                SvnServerUrl = Model.Url
            });
        }

        #endregion

        private async void checkout(CheckoutRepositoryRequest request)
        {
            var svnClientService = new SvnClientService();
            await Task.Run(() => svnClientService.CheckoutRepository(request));
        }
    }
}
