using System;
using System.Collections.Generic;
using System.Text;
using Avalonia.Controls.Notifications;

namespace KiCadDbLib.Views
{
    public static class INotificationManagerExtension
    {
        public static void ShowError(this INotificationManager notificationManager, string title, string message)
        {
            notificationManager.Show(new Notification(title, message, NotificationType.Error, TimeSpan.Zero));
        }

        public static void ShowInformation(this INotificationManager notificationManager, string title, string message)
        {
            notificationManager.Show(new Notification(title, message, NotificationType.Information));
        }

        public static void ShowSuccess(this INotificationManager notificationManager, string title, string message)
        {
            notificationManager.Show(new Notification(title, message, NotificationType.Success));
        }

        public static void ShowWarning(this INotificationManager notificationManager, string title, string message)
        {
            notificationManager.Show(new Notification(title, message, NotificationType.Warning));
        }
    }
}