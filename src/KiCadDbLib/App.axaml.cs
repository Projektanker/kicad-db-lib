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
        /// <inheritdoc/>
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        /// <inheritdoc/>
        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow()
                {
                    DataContext = Locator.Current.GetRequiredService<MainWindowViewModel>(),
                };

                Locator.CurrentMutable.RegisterConstant(desktop.MainWindow);
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}