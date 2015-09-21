using System;
using System.Windows;
using System.Windows.Controls;
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
            var values = Enum.GetValues(typeof (SubversionServerType));
            foreach (var value in values)
            {
                SvnTypeDropdown.Items.Add(value);
            }
        }

        private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            var svnServerSettings = (Models.SvnServer)DataContext;
            svnServerSettings.SetPassword(PasswordBox.Password);

            var a = svnServerSettings.GetPassword();
        }

    }
}
