using System;
using System.Linq;
using System.Windows;
using MahApps.Metro.Controls;
using ReVersion.Models.Settings;

namespace ReVersion.Views
{
    /// <summary>
    ///     Interaction logic for Settings.xaml
    /// </summary>
    public partial class SettingsWindow : MetroWindow
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void SettingsWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            NamingConvention_ComboBox.ItemsSource =
                Enum.GetValues(typeof (SvnNamingConvention)).Cast<SvnNamingConvention>();
        }
        
    }
}