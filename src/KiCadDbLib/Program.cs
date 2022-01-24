using System.Reflection;
using Avalonia;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using KiCadDbLib.Services;
using KiCadDbLib.Services.KiCad.LibraryReader;
using KiCadDbLib.Services.KiCad.LibraryWriter;
using KiCadDbLib.ViewModels;
using KiCadDbLib.Views;
using Projektanker.Icons.Avalonia;
using Projektanker.Icons.Avalonia.MaterialDesign;
using ReactiveUI;
using SimpleInjector;
using Splat;

namespace KiCadDbLib
{
    internal static class Program
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
            IconProvider.Register<MaterialDesignIconProvider>();

            var container = new Container
            {
                Options =
                {
                    EnableAutoVerification = false
                }
            };

            // Services
            container.RegisterSingleton<IPartRepository, PartRepository>();
            container.Register<ILibraryBuilder, LibraryBuilder>();
            container.RegisterDecorator<ILibraryBuilder, LibraryBuilderSingletonProxy>(Lifestyle.Singleton);
            container.RegisterSingleton<ISettingsProvider, SettingsProvider>();

            container.RegisterSingleton<ILibraryReader, KiCadLibraryReaderMediator>();
            container.RegisterDecorator<ILibraryReader, KiCadLibraryReaderCache>(Lifestyle.Singleton);
            container.Register<ILibraryWriterFactory, KiCadLibraryWriterFactory>();

            container.RegisterSingleton<INotificationPoster, SnackbarNotificationPoster>();

            // View model
            container.RegisterSingleton<MainWindowViewModel>();

            // Register views
            container.Register(typeof(IViewFor<>), Assembly.GetCallingAssembly());
            container.RegisterInitializer<IHosted>(hosted => hosted.Host = Locator.Current.GetRequiredService<Window>());

            // Chain them up
            var chain = new LocatorChain(Locator.GetLocator(), container);
            Locator.SetLocator(chain);

            return AppBuilder
                .Configure<App>()
                .UseReactiveUI()
                .UsePlatformDetect()
                .LogToTrace();
        }
    }
}