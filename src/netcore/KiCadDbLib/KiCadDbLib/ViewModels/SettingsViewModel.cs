using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Reflection.Metadata.Ecma335;
using Avalonia.Rendering.SceneGraph;
using KiCadDbLib.Models;
using KiCadDbLib.Services;
using ReactiveUI;

namespace KiCadDbLib.ViewModels
{
    public sealed class SettingsViewModel : RoutableViewModelBase
    {
        private readonly SettingsService _settingsService;
        ObservableAsPropertyHelper<ObservableCollection<SettingsCustomFieldViewModel>> _customFieldsProperty;
        ObservableAsPropertyHelper<SettingsCustomFieldViewModel> _newCustomFieldProperty;
        ObservableAsPropertyHelper<Settings> _settingsProperty;
      
        public SettingsViewModel(IScreen hostScreen, SettingsService settingsService)
            : base(hostScreen)
        {
            _settingsService = settingsService;
        }

        public ObservableCollection<SettingsCustomFieldViewModel> CustomFields => _customFieldsProperty?.Value;

        public ReactiveCommand<Unit, Unit> GoBack => HostScreen.Router.NavigateBack;

        public SettingsCustomFieldViewModel NewCustomField => _newCustomFieldProperty?.Value;

        public Settings Settings => _settingsProperty?.Value;

        public string SettingsLocation => _settingsService?.Location;

        protected override void WhenActivated(CompositeDisposable disposables)
        {
            base.WhenActivated(disposables);

            IObservable<Settings> settingsObservable = _settingsService.GetSettings().ToObservable();

            // Logging
            settingsObservable
                .Subscribe(
                    onNext: _ => Console.WriteLine("Settings: onNext"),
                    onError: exception => Console.WriteLine(exception))
                .DisposeWith(disposables);


            // Settings
            _settingsProperty = settingsObservable
                .ToProperty(this, vm => vm.Settings)
                .DisposeWith(disposables);

            // Custom Fields
            _customFieldsProperty = settingsObservable
                .Select(settings => settings.CustomFields)
                .Select(customFields =>
                {
                    var customFieldsCollection = new ObservableCollection<SettingsCustomFieldViewModel>();
                    foreach (var field in customFields)
                    {
                        customFieldsCollection.Add(new SettingsCustomFieldViewModel(customFieldsCollection, field));
                    }

                    return customFieldsCollection;
                })
                .ToProperty(this, vm => vm.CustomFields)
                .DisposeWith(disposables);

            _newCustomFieldProperty = this.WhenAnyValue(vm => vm.CustomFields)
                .Where(x => x != null)
                .Select(cusomFields => new SettingsCustomFieldViewModel(cusomFields, null))
                .ToProperty(this, vm => vm.NewCustomField)
                .DisposeWith(disposables);


        }
    }
}