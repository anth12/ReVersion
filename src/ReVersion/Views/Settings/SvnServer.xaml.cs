using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ReVersion.Services.Settings;
using ReVersion.Services.Subversion;

namespace ReVersion.Views.Settings
{
    /// <summary>
    /// Interaction logic for SvnServer.xaml
    /// </summary>
    public partial class SvnServer : UserControl
    {
        public SvnServer()
        {
            InitializeComponent();
        }

        private void SvnServer_OnLoaded(object sender, RoutedEventArgs e)
        {
            SvnTypeDropdown.ItemsSource = Enum.GetValues(typeof(SubversionServerType)).Cast<SubversionServerType>();
        }

        private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            var svnServerSettings = (Models.SvnServer)DataContext;
            svnServerSettings.SetPassword(PasswordBox.Password);
        }

        private void Remove_OnClick(object sender, RoutedEventArgs e)
        {
            var svnServerSettings = (Models.SvnServer)DataContext;

            var server = SettingsService.Current.Servers.First(s => s.Id == svnServerSettings.Id);
            SettingsService.Current.Servers.Remove(server);
        }
    }
}
