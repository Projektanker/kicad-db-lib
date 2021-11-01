using System.Reactive;
using System.Reflection;
using KiCadDbLib.Services;
using ReactiveUI;
using Splat;

namespace KiCadDbLib.ViewModels
{
    public sealed class AboutViewModel : RoutableViewModelBase
    {
        private readonly SettingsService _settingsService;

        public AboutViewModel(IScreen hostScreen)
            : base(hostScreen)
        {
            _settingsService = Locator.Current.GetService<SettingsService>();
            GoBack = HostScreen.Router.NavigateBack;
            var entryAssembly = Assembly.GetEntryAssembly();
            License = "MIT";
            GitHub = @"https://github.com/projektanker/kicad-db-lib";
            Version = entryAssembly
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                .InformationalVersion;
        }

        public string GitHub { get; }

        public ReactiveCommand<Unit, IRoutableViewModel?> GoBack { get; }

        public string License { get; }

        public string SettingsLocation => _settingsService?.Location;

        public string Version { get; }
    }
}