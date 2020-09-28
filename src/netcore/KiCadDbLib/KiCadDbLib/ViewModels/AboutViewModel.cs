using System;
using System.Reactive;
using KiCadDbLib.Services;
using ReactiveUI;

namespace KiCadDbLib.ViewModels
{
    public sealed class AboutViewModel : RoutableViewModelBase
    {
        private readonly SettingsService _settingsService;

        public AboutViewModel(IScreen hostScreen, SettingsService settingsService)
            : base(hostScreen)
        {
            _settingsService = settingsService;
            GoBack = HostScreen.Router.NavigateBack;
            License = "MIT";
            GitHub = @"https://github.com/projektanker";
            Version = new Version(1, 4, 0, 4);
        }

        public string GitHub { get; }
        public ReactiveCommand<Unit, Unit> GoBack { get; }

        public string License { get; }
        public string SettingsLocation => _settingsService?.Location;
        public Version Version { get; }
    }
}