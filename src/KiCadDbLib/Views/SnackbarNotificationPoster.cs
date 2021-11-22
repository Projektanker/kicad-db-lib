using KiCadDbLib.Services;
using Material.Styles;

namespace KiCadDbLib.Views
{
    public class SnackbarNotificationPoster : INotificationPoster
    {
        public void ShowError(string title, string message)
        {
            SnackbarHost.Post($"❌ {title}: {message}");
        }

        public void ShowInformation(string title, string message)
        {
            SnackbarHost.Post($"ℹ️ {title}: {message}");
        }

        public void ShowSuccess(string title, string message)
        {
            SnackbarHost.Post($"✔️ {title}: {message}");
        }

        public void ShowWarning(string title, string message)
        {
            SnackbarHost.Post($"⚠️ {title}: {message}");
        }
    }
}