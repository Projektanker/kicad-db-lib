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
using System.Threading.Tasks;
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
        private string _newCustomField;
        private string _databasePath;
        private string _footprintsPath;
        private string _outputPath;
        private string _symbolsPath;

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
            SaveSettings = ReactiveCommand.CreateFromTask(execute: ExecuteSaveSettings);
            GoBack = ReactiveCommand.CreateCombined(new[] {
                SaveSettings,
                HostScreen.Router.NavigateBack
            });
        }

        private Task ExecuteSaveSettings()
        {
            Settings settings = new Settings()
            {
                DatabasePath = DatabasePath,
                FootprintsPath = FootprintsPath,
                OutputPath = OutputPath,
                SymbolsPath = SymbolsPath
            };

            settings.CustomFields.AddRange(
                CustomFields.Select(vm => vm.Value));
            
            return _settingsService.SetSettingsAsync(settings);
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

        public CombinedReactiveCommand<Unit, Unit> GoBack { get; }
        public ReactiveCommand<Unit, Unit> AddCustomField { get; }

        public ReactiveCommand<Unit, Unit> SaveSettings { get; }

        public string DatabasePath
        {
            get => _databasePath;
            set => this.RaiseAndSetIfChanged(ref _databasePath, value);
        }
        public string FootprintsPath
        {
            get => _footprintsPath;
            set => this.RaiseAndSetIfChanged(ref _footprintsPath, value);
        }
        public string OutputPath
        {
            get => _outputPath;
            set => this.RaiseAndSetIfChanged(ref _outputPath, value);
        }
        public string SymbolsPath
        {
            get => _symbolsPath;
            set => this.RaiseAndSetIfChanged(ref _symbolsPath, value);
        }

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
            settingsObservable
                .Do(s => DatabasePath = s.DatabasePath)
                .Do(s => FootprintsPath = s.FootprintsPath)
                .Do(s => OutputPath = s.OutputPath)
                .Do(s => SymbolsPath = s.SymbolsPath)
                .Subscribe()
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