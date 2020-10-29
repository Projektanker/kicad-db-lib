using Avalonia;
using Avalonia.Controls.Notifications;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using KiCadDbLib.ViewModels;
using ReactiveUI;
using Splat;

namespace KiCadDbLib.Views
{
    public class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        private WindowNotificationManager _windowNotificationManager;

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif

            this.WhenActivated(disposables =>
            {
            });
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            _windowNotificationManager = new WindowNotificationManager(this)
            {
                MaxItems = 10,
                Position = NotificationPosition.BottomLeft,
            };

            Locator.CurrentMutable.RegisterConstant<INotificationManager>(_windowNotificationManager);
        }
    }
}