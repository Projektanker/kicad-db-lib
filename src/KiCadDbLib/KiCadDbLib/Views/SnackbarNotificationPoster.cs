using KiCadDbLib.Services;
using Material.Styles;

namespace KiCadDbLib.Views
{
    public class SnackbarNotificationPoster : INotificationPoster
    {
        public void ShowError(string title, string message)
        {
            SnackbarHost.Post($"{title}: {message}");

            //otificationManager.Show(new Notification(title, message, NotificationType.Error, TimeSpan.Zero));
        }

        public void ShowInformation(string title, string message)
        {
            SnackbarHost.Post($"{title}: {message}");

            //notificationManager.Show(new Notification(title, message, NotificationType.Information));
        }

        public void ShowSuccess(string title, string message)
        {
            SnackbarHost.Post($"{title}: {message}");

            //notificationManager.Show(new Notification(title, message, NotificationType.Success));
        }

        public void ShowWarning(string title, string message)
        {
            SnackbarHost.Post($"{title}: {message}");

            //notificationManager.Show(new Notification(title, message, NotificationType.Warning));
        }
    }
}