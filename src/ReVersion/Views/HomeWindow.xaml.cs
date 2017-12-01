using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using MahApps.Metro.Controls;
using ReVersion.Services.Analytics;
using ReVersion.ViewModels.Home;
using Application = System.Windows.Application;

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

        private void HomeWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            //Finally, load the data
            ((HomeViewModel)DataContext).LoadRepositories();
        }

        private void HomeWindow_OnClosed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
        
        #endregion

        private void HomeWindow_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            int cols;
            if (e.NewSize.Width < 500)
                cols = 1;
            else if (e.NewSize.Width < 900)
                cols = 2;
            else
                cols = 3;

            ((HomeViewModel) DataContext).Model.Window.ColumnWidth = (int) Math.Floor(e.NewSize.Width / cols) - 2;
        }

    }
}