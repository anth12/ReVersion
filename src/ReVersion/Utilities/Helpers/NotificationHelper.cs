using System.IO;
using Windows.Foundation;
using Windows.UI.Notifications;
using ReVersion.Services;

namespace ReVersion.Utilities.Helpers
{
    internal class NotificationHelper
    {
        private const string AppId = "ReVersion";

        public static void ShowResult(Result result)
        {
            foreach (var message in result.Messages)
            {
                Show(message);
            }
        }

        public static void Show(
            string title, 
            string message = null,
            string furtherMessage = null,
            TypedEventHandler<ToastNotification, object> onActivate = null,
            TypedEventHandler<ToastNotification, ToastDismissedEventArgs> onDismiss = null)
        {

            // Get a toast XML template
            var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText01);

            // Fill in the text elements
            var stringElements = toastXml.GetElementsByTagName("text");

            var textIndex = 0;
            //Add the title
            stringElements[textIndex].AppendChild(toastXml.CreateTextNode(title));
            textIndex++;

            //Add the message
            if (!string.IsNullOrEmpty(message))
            {
                stringElements[textIndex].AppendChild(toastXml.CreateTextNode(message));
                textIndex++;
            }

            //Add the further message
            if (!string.IsNullOrEmpty(furtherMessage))
            {
                stringElements[textIndex].AppendChild(toastXml.CreateTextNode(furtherMessage));
            }

            // Specify the absolute path to an image
            var imagePath = "file:///" + Path.GetFullPath("Resources/logo_512.png");
            var imageElements = toastXml.GetElementsByTagName("image");
            var namedItem = imageElements[0].Attributes.GetNamedItem("src");

            if (namedItem != null)
                namedItem.NodeValue = imagePath;


            var toast = new ToastNotification(toastXml);

            if (onActivate != null)
            {
                toast.Activated += onActivate;
            }

            if (onDismiss != null)
            {
                toast.Dismissed += onDismiss;
            }

            ToastNotificationManager.CreateToastNotifier(AppId).Show(toast);
        }
        
    }
}