using System.Reflection;
using Avalonia;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using KiCadDbLib.Services;
using KiCadDbLib.Services.KiCad;
using KiCadDbLib.ViewModels;
using Projektanker.Icons.Avalonia;
using Projektanker.Icons.Avalonia.FontAwesome;
using ReactiveUI;
using SimpleInjector;
using Splat;

namespace KiCadDbLib
{
    internal class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        public static void Main(string[] args)
        {
            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
        {
            Container container = new Container();
            container.Options.AllowOverridingRegistrations = true;
            container.Options.EnableAutoVerification = false;

            // Services
            container.RegisterSingleton<SettingsService>();
            container.RegisterSingleton<PartsService>();
            container.RegisterSingleton<KiCadLibraryReader>();

            // View model
            container.RegisterSingleton<MainWindowViewModel>();
            container.RegisterSingleton<PartsViewModel>();
            container.RegisterSingleton<SettingsViewModel>();
            container.RegisterSingleton<AboutViewModel>();

            // Register views
            container.Register(typeof(IViewFor<>), Assembly.GetCallingAssembly());
            container.RegisterInitializer<IHosted>(hosted => hosted.Host = Locator.Current.GetService<Window>());

            // Chain them up
            var chain = new LocatorChain(Locator.GetLocator(), container);
            Locator.SetLocator(chain);

            return AppBuilder
                .Configure<App>()
                .AfterSetup(AfterSetupCallback)
                .UseReactiveUI()
                .UsePlatformDetect()
                .LogToTrace();
        }

        private static void AfterSetupCallback(AppBuilder appBuilder)
        {
            IconProvider.Register<FontAwesomeIconProvider>();
        }
    }
}