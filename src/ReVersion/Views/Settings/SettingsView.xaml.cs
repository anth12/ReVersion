using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ReVersion.Models.Settings;

namespace ReVersion.Views.Settings
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();
        }

        private void SettingsView_OnLoaded(object sender, RoutedEventArgs e)
        {
            NamingConvention_ComboBox.ItemsSource =
                Enum.GetValues(typeof(SvnNamingConvention)).Cast<SvnNamingConvention>();
        }
    }
}
