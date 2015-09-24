using System.Windows;
using MahApps.Metro.Controls;
using ReVersion.Models;
using ReVersion.Services.Settings;

namespace ReVersion.Views
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class SettingsWindow : MetroWindow
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }
        
        private void SettingsWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = SettingsService.Current;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            SettingsService.Current.Servers.Add(new SvnServerModel());
        }
    }
}
