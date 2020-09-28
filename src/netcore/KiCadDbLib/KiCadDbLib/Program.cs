using System.Reflection;
using Avalonia;
using Avalonia.Logging.Serilog;
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
            container.RegisterSingleton<PartsViewModel>();
            container.RegisterSingleton<SettingsViewModel>();
            container.RegisterSingleton<AboutViewModel>();

            // Router uses Splat.Locator to resolve views for view models, so we need to register
            // our views. Locator.CurrentMutable.Register(() => new PartsView(),
            // typeof(IViewFor<PartsViewModel>)); Locator.CurrentMutable.Register(() => new
            // SettingsView(), typeof(IViewFor<SettingsViewModel>));
            Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetCallingAssembly());

            // Chain them up
            var chain = new LocatorChain(Locator.GetLocator(), container);
            Locator.SetLocator(chain);

            return AppBuilder.Configure<App>()
                .AfterSetup(AfterSetupCallback)
                .UseReactiveUI()
                .UsePlatformDetect()
                .LogToDebug();
        }

        private static void AfterSetupCallback(AppBuilder appBuilder)
        {
            IconProvider.Register<FontAwesomeIconProvider>();
        }
    }
}