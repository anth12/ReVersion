using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ReVersion.Services.Settings;
using ReVersion.Services.SvnClient;
using ReVersion.Services.SvnClient.Requests;
using ReVersion.Services.SvnServer.Response;

namespace ReVersion.Views.Repository
{
    /// <summary>
    ///     Interaction logic for Repository.xaml
    /// </summary>
    public partial class Repository : UserControl
    {
        public Repository()
        {
            InitializeComponent();
        }

        private void Checkout_Click(object sender, RoutedEventArgs e)
        {
            var data = (RepositoryResult) DataContext;

            var svnServer = SettingsService.Current.Servers.First(s => s.Id == data.SvnServerId);

            Checkout(new CheckoutRepositoryRequest
            {
                SvnUsername = svnServer.Username,
                SvnPassword = svnServer.GetPassword(),
                ProjectName = data.Name,
                SvnServerUrl = data.Url
            });
        }

        private async void Checkout(CheckoutRepositoryRequest request)
        {
            var svnClientService = new SvnClientService();
            await Task.Run(()=> svnClientService.CheckoutRepository(request));
        }
    }
}