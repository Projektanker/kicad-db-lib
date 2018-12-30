using System;
using Avalonia;
using Avalonia.Logging.Serilog;
using KiCadDbLib.ViewModels;
using KiCadDbLib.Views;

namespace KiCadDbLib
{
    class Program
    {
        static void Main(string[] args)
        {
            BuildAvaloniaApp().Start<MainWindow>(() => new MainWindowViewModel());
        }

        public static AppBuilder BuildAvaloniaApp()
        {
            return AppBuilder.Configure<App>()
                           .UsePlatformDetect()
                           .UseReactiveUI()
                           .LogToDebug();
        }
    }
}
