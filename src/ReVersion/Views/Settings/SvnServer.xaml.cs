using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ReVersion.Models;
using ReVersion.Models.Settings;
using ReVersion.Services.Settings;
using ReVersion.Services.SvnServer;
using ReVersion.ViewModels.Settings;

namespace ReVersion.Views.Settings
{
    /// <summary>
    ///     Interaction logic for SvnServer.xaml
    /// </summary>
    public partial class SvnServer : UserControl
    {
        public SvnServer()
        {
            InitializeComponent();
        }

        private void SvnServer_OnLoaded(object sender, RoutedEventArgs e)
        {
            SvnTypeDropdown.ItemsSource = Enum.GetValues(typeof (SvnServerType)).Cast<SvnServerType>();
            Password_Textbox.Password = ((SvnServerViewModel)DataContext).Model.RawPassword;
        }

        private void Password_Textbox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            ((SvnServerViewModel) DataContext).Model.RawPassword = Password_Textbox.Password;
        }
    }
}