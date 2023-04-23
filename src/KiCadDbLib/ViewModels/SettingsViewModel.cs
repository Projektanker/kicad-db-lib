using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
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
using Microsoft.CodeAnalysis;
using ReactiveUI;
using Splat;

namespace KiCadDbLib.ViewModels
{
    public sealed class SettingsViewModel : RoutableViewModelBase, IDisposable
    {
        private readonly ISettingsProvider _settingsProvider;
        private readonly IPartRepository _partsRepository;
        private ObservableAsPropertyHelper<ObservableCollection<SettingsCustomFieldViewModel>> _customFieldsProperty;
        private ObservableAsPropertyHelper<FormGroup> _pathsFormProperty;
        private string _newCustomField = string.Empty;

        public SettingsViewModel(IScreen hostScreen)
            : base(hostScreen)
        {
            _settingsProvider = Locator.Current.GetRequiredService<ISettingsProvider>();
            _partsRepository = Locator.Current.GetRequiredService<IPartRepository>();

            _customFieldsProperty = ObservableAsPropertyHelper<ObservableCollection<SettingsCustomFieldViewModel>>
                .Default(new ObservableCollection<SettingsCustomFieldViewModel>());

            _pathsFormProperty = ObservableAsPropertyHelper<FormGroup>
                .Default(new FormGroup());

            SaveSettings = ReactiveCommand.CreateFromTask(execute: ExecuteSaveSettings);

            var canAddCustomField = this.WhenAnyValue(vm => vm.NewCustomField, value =>
            {
                return !string.IsNullOrWhiteSpace(value)
                    && !CustomFields.Any(vm => vm.Value.Equals(value, StringComparison.Ordinal));
            });

            AddCustomField = ReactiveCommand.Create(execute: ExecuteAddCustomField, canExecute: canAddCustomField);

            ImportCustomFields = ReactiveCommand.CreateCombined(new[]
            {
                SaveSettings,
                ReactiveCommand.CreateFromTask(
                    execute: ImportCustomFieldsAsync),
            });

            GoBack = ReactiveCommand.CreateFromTask(GoBackAsync);

            SelectFolder = new Interaction<string, string?>();
        }

        public ReactiveCommand<Unit, Unit> AddCustomField { get; }

        public CombinedReactiveCommand<Unit, Unit> ImportCustomFields { get; }

        public ObservableCollection<SettingsCustomFieldViewModel> CustomFields => _customFieldsProperty.Value;

        public ReactiveCommandBase<Unit, Unit> GoBack { get; }

        public Interaction<string, string?> SelectFolder { get; }

        public string NewCustomField
        {
            get => _newCustomField;
            set => this.RaiseAndSetIfChanged(ref _newCustomField, value);
        }

        public AbstractControl PathsForm => _pathsFormProperty.Value;

        public ReactiveCommand<Unit, Unit> SaveSettings { get; }

        public string SettingsLocation => _settingsProvider.Location;

        public void Dispose()
        {
            _pathsFormProperty.Dispose();
            _customFieldsProperty.Dispose();
        }

        /// <inheritdoc/>
        protected override void WhenActivated(CompositeDisposable disposables)
        {
            base.WhenActivated(disposables);

            IObservable<WorkspaceSettings> settingsObservable = _settingsProvider.GetWorkspaceSettings().ToObservable();

            // Logging
            settingsObservable
                .Subscribe(
                    onNext: _ => Console.WriteLine("Settings: onNext"),
                    onError: exception => Console.WriteLine(exception))
                .DisposeWith(disposables);

            // Settings Paths
            _pathsFormProperty = settingsObservable
                .Select(GetPathsForm)
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

        private static FormGroup GetPathsForm(WorkspaceSettings settings)
        {
            var form = new FormGroup();

            form.Add(nameof(WorkspaceSettings.DatabasePath), new FormControl(settings.DatabasePath)
            {
                Label = "Database",
                Validator = Validators.DirectoryExists,
            });

            form.Add(nameof(WorkspaceSettings.SymbolsPath), new FormControl(settings.SymbolsPath)
            {
                Label = "Symbols",
                Validator = Validators.DirectoryExists,
            });

            form.Add(nameof(WorkspaceSettings.FootprintsPath), new FormControl(settings.FootprintsPath)
            {
                Label = "Footprints",
                Validator = Validators.DirectoryExists,
            });

            form.Add(nameof(WorkspaceSettings.OutputPath), new FormControl(settings.OutputPath)
            {
                Label = "Output",
                Validator = Validators.DirectoryExists,
            });

            return form;
        }

        private async Task GoBackAsync()
        {
            await ExecuteSaveSettings()
                .ConfigureAwait(true);
            await HostScreen.Router.NavigateBack.Execute();
        }

        private void ExecuteAddCustomField()
        {
            CustomFields.Add(new SettingsCustomFieldViewModel(NewCustomField, RemoveCustomField));
            NewCustomField = string.Empty;
        }

        private Task ExecuteSaveSettings()
        {
            if (!PathsForm!.Validate())
            {
                throw new ValidationException("Settings are invalid.");
            }

            var settings = PathsForm.GetValue<WorkspaceSettings>();

            settings.CustomFields.AddRange(
                CustomFields.Select(vm => vm.Value));

            return _settingsProvider.UpdateWorkspaceSettings(settings);
        }

        private void RemoveCustomField(string customField)
        {
            var vm = CustomFields.Single(vm => vm.Value.Equals(customField, StringComparison.Ordinal));
            CustomFields.Remove(vm);
        }

        private async Task ImportCustomFieldsAsync()
        {
            var parts = await _partsRepository.GetPartsAsync()
                .ConfigureAwait(true);

            var customFieldsFromParts = parts
                .SelectMany(part => part.CustomFields)
                .Where(kv => !string.IsNullOrWhiteSpace(kv.Value))
                .Select(kv => kv.Key)
                .Distinct();

            var customFieldVms = CustomFields
                .Select(vm => vm.Value)
                .Concat(customFieldsFromParts)
                .Distinct()
                .OrderBy(s => s)
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