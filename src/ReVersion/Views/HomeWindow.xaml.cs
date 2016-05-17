using System;
using System.IO;
using System.Linq;
using System.Windows;
using MahApps.Metro.Controls;
using ReVersion.Services.Analytics;
using ReVersion.Services.SvnClient;

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
            
            var screenName = this.GetType().Name;

            try
            {
                AnalyticsService.Session.CreatePageViewRequest(screenName, screenName).Send();
            }
            catch (Exception)
            {
                //Fail silently
            }

            using (var svnClient = new SvnClientService())
            {
                // Create a new client to fore clear credentials
                // TODO remove
            }
        }
        
        #region Window Open/Close events


        private void HomeWindow_OnClosed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
        
        #endregion
        
    }
}