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
using ReVersion.Models;
using SubversionServerType = ReVersion.Services.Subversion.SubversionServerType;

namespace ReVersion.Views
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }
        
        private void SettingsWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = new SettingsModel
            {
                Servers = new ObservableCollection<SvnServer>
                {
                    new SvnServer { BaseUrl = "http://svn.example.com", Type = SubversionServerType.Submin},
                    new SvnServer { BaseUrl = "http://sven/", Type = SubversionServerType.Sven}
                }
            };
        }
    }
}
