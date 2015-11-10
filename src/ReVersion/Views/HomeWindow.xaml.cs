using System;
using System.Windows;
using MahApps.Metro.Controls;
using ReVersion.Services.Analytics;

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
        }
        
        #region Window Open/Close events


        private void HomeWindow_OnClosed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
        
        #endregion
        
    }
}