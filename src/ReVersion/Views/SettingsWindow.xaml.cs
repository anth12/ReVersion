using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using Newtonsoft.Json;
using ReVersion.Models;
using ReVersion.Services.Settings;
using SubversionServerType = ReVersion.Services.Subversion.SubversionServerType;

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
            SettingsService.Current.Servers.Add(new SvnServer());
        }
    }
}
