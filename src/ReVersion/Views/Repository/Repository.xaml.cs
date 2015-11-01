using System.Linq;
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

            var svnClientService = new SvnClientService();
            svnClientService.CheckoutRepository(new CheckoutRepositoryRequest
            {
                SvnUsername = svnServer.Username,
                SvnPassword = svnServer.GetPassword(),
                ProjectName = data.Name,
                SvnServerUrl = data.Url
            });
        }
    }
}