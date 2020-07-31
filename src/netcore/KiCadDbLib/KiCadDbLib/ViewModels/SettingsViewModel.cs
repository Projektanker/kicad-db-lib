using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive;
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
        private ObservableCollection<SettingsCustomFieldViewModel> customFields;
        private SettingsCustomFieldViewModel newCustomField;
        private Settings settings;

        public SettingsViewModel(IScreen hostScreen, SettingsService settingsService)
        {
            HostScreen = hostScreen ?? throw new ArgumentNullException(nameof(hostScreen));
            _settingsService = settingsService;
            UrlPathSegment = Guid.NewGuid().ToString().Substring(0, 5);
            this.WhenNavigatedTo(OnNavigatedTo);
        }

        public ObservableCollection<SettingsCustomFieldViewModel> CustomFields
        {
            get => customFields;
            set => this.RaiseAndSetIfChanged(ref customFields, value);
        }

        public ReactiveCommand<Unit, Unit> GoBack => HostScreen.Router.NavigateBack;

        /// <summary>
        /// Gets the <see cref="IScreen"/> that owns the routable view model.
        /// </summary>
        public IScreen HostScreen { get; }

        public SettingsCustomFieldViewModel NewCustomField
        {
            get => newCustomField;
            private set => this.RaiseAndSetIfChanged(ref newCustomField, value);
        }

        public Settings Settings
        {
            get => settings;
            private set => this.RaiseAndSetIfChanged(ref settings, value);
        }

        public string SettingsLocation => _settingsService?.Location;

        /// <summary>
        /// Gets the unique identifier for the routable view model.
        /// </summary>
        public string UrlPathSegment { get; }

        public void Dispose()
        {
            settings = null;
        }

        private IDisposable OnNavigatedTo()
        {
            _settingsService.GetSettings().ToObservable().Subscribe(
                onNext: settings =>
                {
                    Settings = settings;
                    var customFields = new ObservableCollection<SettingsCustomFieldViewModel>();
                    foreach (var field in settings.CustomFields)
                    {
                        customFields.Add(new SettingsCustomFieldViewModel(customFields, field));
                    }
                    CustomFields = customFields;
                    NewCustomField = new SettingsCustomFieldViewModel(customFields, null);
                },
                onError: exception => Debug.WriteLine(exception));

            return this;
        }
    }
}