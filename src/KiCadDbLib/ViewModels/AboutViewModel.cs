using System.Reactive;
using System.Reflection;
using KiCadDbLib.Services;
using ReactiveUI;
using Splat;

namespace KiCadDbLib.ViewModels
{
    public sealed class AboutViewModel : RoutableViewModelBase
    {
        private readonly ISettingsProvider _settingsProvider;

        public AboutViewModel(IScreen hostScreen)
            : base(hostScreen)
        {
            _settingsProvider = Locator.Current.GetService<ISettingsProvider>()!;
            GoBack = HostScreen.Router.NavigateBack;
            License = "MIT";
            GitHub = @"https://github.com/projektanker/kicad-db-lib";
            Version = Assembly.GetEntryAssembly()
                !.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                !.InformationalVersion;
        }

        public string GitHub { get; }

        public ReactiveCommand<Unit, IRoutableViewModel?> GoBack { get; }

        public string License { get; }

        public string? SettingsLocation => _settingsProvider?.Location;

        public string Version { get; }
    }
}