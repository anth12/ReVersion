using System;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
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
            NamingConvension_ComboBox.ItemsSource = Enum.GetValues(typeof(SvnNamingConvension)).Cast<SvnNamingConvension>();
            this.DataContext = SettingsService.Current;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            SettingsService.Current.Servers.Add(new SvnServerModel());
        }

        private void CheckoutFolder_FolderPicker_OnClick(object sender, RoutedEventArgs e)
        {
            var folderPicker = new FolderBrowserDialog
            {
                SelectedPath = CheckoutFolder_Textbox.Text
            };

            var result = folderPicker.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                ((SettingsModel)DataContext).CheckoutFolder = folderPicker.SelectedPath;
            }

        }
    }
}
