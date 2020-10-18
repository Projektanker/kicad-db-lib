using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using DynamicData;
using KiCadDbLib.Models;
using KiCadDbLib.ReactiveForms;
using KiCadDbLib.ReactiveForms.Validation;
using KiCadDbLib.Services;
using ReactiveUI;
using Splat;

namespace KiCadDbLib.ViewModels
{
    public sealed class SettingsViewModel : RoutableViewModelBase
    {
        private readonly SettingsService _settingsService;
        private readonly PartsService _partsService;
        private ObservableAsPropertyHelper<ObservableCollection<SettingsCustomFieldViewModel>> _customFieldsProperty;
        private string _newCustomField;
        private ObservableAsPropertyHelper<FormGroup> _pathsFormProperty;

        public SettingsViewModel(IScreen hostScreen)
            : base(hostScreen)
        {
            _settingsService = Locator.Current.GetService<SettingsService>();
            _partsService = Locator.Current.GetService<PartsService>(); ;

            SaveSettings = ReactiveCommand.CreateFromTask(execute: ExecuteSaveSettings);

            var canAddCustomField = this.WhenAnyValue(vm => vm.NewCustomField, value =>
            {
                return !string.IsNullOrWhiteSpace(value)
                    && !CustomFields.Any(vm => vm.Value.Equals(value, StringComparison.CurrentCulture));
            });

            AddCustomField = ReactiveCommand.Create(execute: ExecuteAddCustomField, canExecute: canAddCustomField);

            ImportCustomFields = ReactiveCommand.CreateCombined(new[]
            {
                SaveSettings,
                ReactiveCommand.CreateFromTask(
                    execute: ImportCustomFieldsAsync),
            });

            GoBack = ReactiveCommand.CreateCombined(new[]
            {
                SaveSettings,
                HostScreen.Router.NavigateBack
            });
        }

        public ReactiveCommand<Unit, Unit> AddCustomField { get; }
        public CombinedReactiveCommand<Unit, Unit> ImportCustomFields { get; }

        public ObservableCollection<SettingsCustomFieldViewModel> CustomFields => _customFieldsProperty?.Value;

        public CombinedReactiveCommand<Unit, Unit> GoBack { get; }

        public string NewCustomField
        {
            get => _newCustomField;
            set => this.RaiseAndSetIfChanged(ref _newCustomField, value);
        }

        public AbstractControl PathsForm => _pathsFormProperty?.Value;

        public ReactiveCommand<Unit, Unit> SaveSettings { get; }

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

            // Settings Paths
            _pathsFormProperty = settingsObservable
                .Select(settings => GetPathsForm(settings))
                .ToProperty(this, nameof(PathsForm))
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

        private static FormGroup GetPathsForm(Settings settings)
        {
            var form = new FormGroup();

            form.Add(nameof(Settings.DatabasePath), new FormControl(settings.DatabasePath)
            {
                Label = "Database",
                Validator = Validators.Compose(
                        Validators.Required,
                        Validators.DirectoryExists)
            });

            form.Add(nameof(Settings.SymbolsPath), new FormControl(settings.SymbolsPath)
            {
                Label = "Symbols",
                Validator = Validators.Compose(
                        Validators.Required,
                        Validators.DirectoryExists)
            });

            form.Add(nameof(Settings.FootprintsPath), new FormControl(settings.FootprintsPath)
            {
                Label = "Footprints",
                Validator = Validators.Compose(
                        Validators.Required,
                        Validators.DirectoryExists)
            });

            form.Add(nameof(Settings.OutputPath), new FormControl(settings.OutputPath)
            {
                Label = "Output",
                Validator = Validators.Compose(
                        Validators.Required,
                        Validators.DirectoryExists)
            });

            return form;
        }

        private void ExecuteAddCustomField()
        {
            CustomFields.Add(new SettingsCustomFieldViewModel(NewCustomField, RemoveCustomField));
            NewCustomField = string.Empty;
        }

        private Task ExecuteSaveSettings()
        {
            Settings settings = PathsForm.GetValue<Settings>();

            settings.CustomFields.AddRange(
                CustomFields.Select(vm => vm.Value));

            return _settingsService.SetSettingsAsync(settings);
        }

        private void RemoveCustomField(string customField)
        {
            SettingsCustomFieldViewModel vm = CustomFields.Single(vm => vm.Value.Equals(customField, StringComparison.CurrentCulture));
            CustomFields.Remove(vm);
        }

        private async Task ImportCustomFieldsAsync()
        {
            IEnumerable<string> customFields = CustomFields.Select(vm => vm.Value);

            var parts = await _partsService.GetPartsAsync();
            customFields = customFields
                .Concat(parts.SelectMany(part => part.CustomFields.Keys))
                .Distinct()
                .OrderBy(s => s);

            SettingsCustomFieldViewModel[] customFieldVms = customFields
                .Select(cf => new SettingsCustomFieldViewModel(cf, RemoveCustomField))
                .ToArray();

            CustomFields.Clear();
            foreach (var item in customFieldVms)
            {
                CustomFields.Add(item);
            }
        }
    }
}