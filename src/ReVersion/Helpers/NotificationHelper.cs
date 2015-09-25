using System;
using System.IO;
using Windows.UI.Notifications;
using ReVersion.Services;

namespace ReVersion.Helpers
{
    public class NotificationHelper
    {
        private const string APP_ID = "ReVersion";

        public static void ShowResult(Result result)
        {
            
            // Get a toast XML template
            var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText01);

            // Fill in the text elements
            var stringElements = toastXml.GetElementsByTagName("text");
            for (int i = 0; i < stringElements.Length; i++)
            {
                stringElements[i].AppendChild(toastXml.CreateTextNode("Line " + i));
            }

            // Specify the absolute path to an image
            var imagePath = "file:///" + Path.GetFullPath("Resources/logo.png");
            var imageElements = toastXml.GetElementsByTagName("image");
            imageElements[0].Attributes.GetNamedItem("src").NodeValue = imagePath;

            // Create the toast and attach event listeners
            var toast = new ToastNotification(toastXml);
            //toast.ExpirationTime = new DateTimeOffset(0, 0, 0, 0, 0, 5, new TimeSpan());

            //toast.Activated += ToastActivated;
            //toast.Dismissed += ToastDismissed;
            //toast.Failed += ToastFailed;
            
            // Show the toast. Be sure to specify the AppUserModelId on your application's shortcut!
            ToastNotificationManager.CreateToastNotifier(APP_ID).Show(toast);
            
        }

        
    }
}
