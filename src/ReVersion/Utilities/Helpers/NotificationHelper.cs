using System.IO;
using Windows.UI.Notifications;
using ReVersion.Services;

namespace ReVersion.Utilities.Helpers
{
    public class NotificationHelper
    {
        private const string APP_ID = "ReVersion";

        public static void ShowResult(Result result)
        {
            foreach (var message in result.Messages)
            {
                ShowToast(message);
            }
        }

        public static void Show(string title)
        {
            ShowToast(title);
        }

        public static void Show(string title, string message)
        {
            ShowToast(title, message);
        }

        public static void Show(string title, string message1, string message2)
        {
            ShowToast(title, message1, message2);
        }

        private static void ShowToast(params string[] messages)
        {
            // Get a toast XML template
            var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText01);

            // Fill in the text elements
            var stringElements = toastXml.GetElementsByTagName("text");

            for (var index = 0; index < messages.Length; index++)
            {
                stringElements[index].AppendChild(toastXml.CreateTextNode(messages[index]));
            }

            // Specify the absolute path to an image
            var imagePath = "file:///" + Path.GetFullPath("Resources/logo_512.png");
            var imageElements = toastXml.GetElementsByTagName("image");
            var namedItem = imageElements[0].Attributes.GetNamedItem("src");

            if (namedItem != null)
                namedItem.NodeValue = imagePath;


            var toast = new ToastNotification(toastXml);

            //toast.Activated += ToastActivated;
            //toast.Dismissed += ToastDismissed;

            ToastNotificationManager.CreateToastNotifier(APP_ID).Show(toast);
        }
    }
}