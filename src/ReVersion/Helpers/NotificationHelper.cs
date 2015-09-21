using System;
using System.Windows.Threading;
using ReVersion.Services;
using ReVersion.Views.Shared;

namespace ReVersion.Helpers
{
    public class NotificationHelper
    {
        public static void ShowResult(Result result)
        {
            App.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(
            () =>
            {
                var notify = new Notification();
                notify.Show();
            }));
        }
    }
}
