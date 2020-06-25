using System;
using System.Reflection;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Logging.Serilog;
using Avalonia.ReactiveUI;
using KiCadDbLib.ViewModels;
using KiCadDbLib.Views;
using ReactiveUI;
using Splat;

namespace KiCadDbLib
{
    class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        public static void Main(string[] args)
        {
             var res = Assembly.GetExecutingAssembly().GetManifestResourceNames();
             res = Assembly.LoadFrom("Fontawesome.Avalonia.dll").GetManifestResourceNames();
            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
        {
            // Router uses Splat.Locator to resolve views for
            // view models, so we need to register our views.
            //
            Locator.CurrentMutable.Register(() => new PartsView(), typeof(IViewFor<PartsViewModel>));
            Locator.CurrentMutable.Register(() => new SettingsView(), typeof(IViewFor<SettingsViewModel>));
            
            return AppBuilder.Configure<App>()
                           .UseReactiveUI()
                           .UsePlatformDetect()
                           .LogToDebug();
        }
    }
}
