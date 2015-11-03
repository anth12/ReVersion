using System;
using System.Linq;
using System.Windows;
using MahApps.Metro.Controls;
using ReVersion.Models;
using ReVersion.Models.Home;
using ReVersion.Services.SvnClient;
using ReVersion.Services.SvnServer;
using ReVersion.Utilities.Helpers;

namespace ReVersion.Views
{
    /// <summary>
    ///     Interaction logic for HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : MetroWindow
    {
        public HomeWindow()
        {
            InitializeComponent();
        }
        
        #region Window Open/Close events


        private void HomeWindow_OnClosed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        #endregion

        
    }
}