using System;
using System.Windows;
using MahApps.Metro.Controls;
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