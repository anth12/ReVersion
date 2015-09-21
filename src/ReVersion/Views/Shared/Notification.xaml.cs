using System;
using System.Windows;
using System.Windows.Threading;
using MahApps.Metro.Controls;

namespace ReVersion.Views.Shared
{
    public partial class Notification : Window
    {
        public Notification()
            : base()
        {
            this.InitializeComponent();
            this.Closed += this.NotificationWindowClosed;
        }

        public new void Show()
        {
            this.Topmost = true;
            base.Show();

            this.Owner = Application.Current.MainWindow;
            this.Closed += this.NotificationWindowClosed;
            
            this.Left = 500 - this.ActualWidth;
            double top = 500 - this.ActualHeight;

            foreach (Window window in System.Windows.Application.Current.Windows)
            {
                string windowName = window.GetType().Name;

                if (windowName.Equals("NotificationWindow") && window != this)
                {
                    window.Topmost = true;
                    top = window.Top - window.ActualHeight;
                }
            }

            this.Top = top;
        }
        private void ImageMouseUp(object sender,
            System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void DoubleAnimationCompleted(object sender, EventArgs e)
        {
            if (!this.IsMouseOver)
            {
                this.Close();
            }
        }

        private void NotificationWindowClosed(object sender, EventArgs e)
        {
            foreach (Window window in System.Windows.Application.Current.Windows)
            {
                string windowName = window.GetType().Name;

                if (windowName.Equals("NotificationWindow") && window != this)
                {
                    // Adjust any windows that were above this one to drop down
                    if (window.Top < this.Top)
                    {
                        window.Top = window.Top + this.ActualHeight;
                    }
                }
            }
        }

    }
}
