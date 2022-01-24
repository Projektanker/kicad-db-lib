using System;

namespace KiCadDbLib.Services
{
    public interface INotificationPoster
    {
        void ShowError(string title, Exception exception);

        void ShowInformation(string title, string message);

        void ShowSuccess(string title, string message);

        void ShowWarning(string title, string message);
    }
}