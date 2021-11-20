namespace KiCadDbLib.Services
{
    public interface INotificationPoster
    {
        void ShowError(string title, string message);

        void ShowInformation(string title, string message);

        void ShowSuccess(string title, string message);

        void ShowWarning(string title, string message);
    }
}