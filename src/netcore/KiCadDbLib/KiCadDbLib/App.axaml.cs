using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using KiCadDbLib.ViewModels;
using KiCadDbLib.Views;
using Splat;

namespace KiCadDbLib
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow()
                {
                    DataContext = Locator.Current.GetService<MainWindowViewModel>(),
                };

                Locator.CurrentMutable.RegisterConstant(desktop.MainWindow);
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}