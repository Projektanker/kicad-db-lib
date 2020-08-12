using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using Avalonia.Rendering.SceneGraph;
using DynamicData;
using KiCadDbLib.Models;
using KiCadDbLib.Services;
using ReactiveUI;

namespace KiCadDbLib.ViewModels
{
    public sealed class SettingsViewModel : RoutableViewModelBase
    {
        private readonly SettingsService _settingsService;
        ObservableAsPropertyHelper<ObservableCollection<SettingsCustomFieldViewModel>> _customFieldsProperty;
        ObservableAsPropertyHelper<Settings> _settingsProperty;
        private string _newCustomField;

        public SettingsViewModel(IScreen hostScreen, SettingsService settingsService)
            : base(hostScreen)
        {
            _settingsService = settingsService;

            var canAddCustomField = this.WhenAnyValue(vm => vm.NewCustomField, value =>
            {
                return !string.IsNullOrWhiteSpace(value) 
                    && !CustomFields.Any(vm => vm.Value.Equals(value, StringComparison.CurrentCulture));
            });

            AddCustomField = ReactiveCommand.Create(execute: ExecuteAddCustomField, canExecute: canAddCustomField);
        }


        public string NewCustomField
        {
            get => _newCustomField;
            set => this.RaiseAndSetIfChanged(ref _newCustomField, value);
        }

        private void ExecuteAddCustomField()
        {
            CustomFields.Add(new SettingsCustomFieldViewModel(NewCustomField, RemoveCustomField));
            NewCustomField = string.Empty;
        }

        public ObservableCollection<SettingsCustomFieldViewModel> CustomFields => _customFieldsProperty?.Value;

        public ReactiveCommand<Unit, Unit> GoBack => HostScreen.Router.NavigateBack;
        public ReactiveCommand<Unit, Unit> AddCustomField { get; }


        public Settings Settings => _settingsProperty?.Value;

        public string SettingsLocation => _settingsService?.Location;

        protected override void WhenActivated(CompositeDisposable disposables)
        {
            base.WhenActivated(disposables);

            IObservable<Settings> settingsObservable = _settingsService.GetSettingsAsync().ToObservable();

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
                    IEnumerable<SettingsCustomFieldViewModel> collection =
                        customFields.Select(cf => new SettingsCustomFieldViewModel(cf, RemoveCustomField));
                    return new ObservableCollection<SettingsCustomFieldViewModel>(collection);
                })
                .ToProperty(this, nameof(CustomFields))
                .DisposeWith(disposables);

        }

        private void RemoveCustomField(string customField)
        {
            SettingsCustomFieldViewModel vm = CustomFields.Single(vm => vm.Value.Equals(customField, StringComparison.CurrentCulture));
            CustomFields.Remove(vm);
        }
    }
}