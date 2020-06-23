using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using KiCadDbLib.Models;
using KiCadDbLib.Services;
using ReactiveUI;

namespace KiCadDbLib.ViewModels
{
    public sealed class SettingsViewModel : ViewModelBase, IRoutableViewModel, IDisposable
    {
        private readonly SettingsService _settingsService;
        private Settings settings;

        public SettingsViewModel(IScreen hostScreen, SettingsService settingsService)
        {
            HostScreen = hostScreen ?? throw new ArgumentNullException(nameof(hostScreen));
            _settingsService = settingsService;
            UrlPathSegment = Guid.NewGuid().ToString().Substring(0, 5);

            this.WhenNavigatedTo(OnNavigatedTo);
        }

        /// <summary>
        /// Gets the <see cref="IScreen"/> that owns the routable view model.
        /// </summary>
        public IScreen HostScreen { get; }

        /// <summary>
        /// Gets the unique identifier for the routable view model.
        /// </summary>
        public string UrlPathSegment { get; }

        public string SettingsLocation => _settingsService?.Location;

        public Settings Settings
        {
            get => settings;
            private set => this.RaiseAndSetIfChanged(ref settings, value);
        }

        public void Dispose()
        {
            settings = null;
        }

        private IDisposable OnNavigatedTo()
        {
            _settingsService.GetSettings().ToObservable().Subscribe(
                onNext: settings => Settings = settings,
                onError: exception => Debug.WriteLine(exception));

            return this;
        }
    }
}