using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Metadata;
using DynamicData.Alias;
using KiCadDbLib.Models;
using KiCadDbLib.Navigation;
using KiCadDbLib.Services;
using ReactiveUI;
using Splat;

namespace KiCadDbLib.ViewModels
{
    public class PartsViewModel : RoutableViewModelBase
    {
        private readonly ISettingsProvider _settingsProvider;
        private readonly IPartRepository _partRepository;

        private ObservableAsPropertyHelper<IEnumerable<ColumnInfo>> _partColumnsProperty;
        private IReadOnlyList<Part> _parts = Array.Empty<Part>();

        public PartsViewModel(IScreen hostScreen)
            : base(hostScreen)
        {
            _settingsProvider = Locator.Current.GetRequiredService<ISettingsProvider>();
            _partRepository = Locator.Current.GetRequiredService<IPartRepository>();
            var libraryBuilder = Locator.Current.GetRequiredService<ILibraryBuilder>();

            _partColumnsProperty = ObservableAsPropertyHelper<IEnumerable<ColumnInfo>>
                .Default(Enumerable.Empty<ColumnInfo>());

            SetWorkspace = ReactiveCommand.CreateFromTask(ExecuteSetWorkspace);

            GoToSettings = NavigationCommand.Create(
                hostScreen: HostScreen,
                viewModelFactory: () => new SettingsViewModel(hostScreen: HostScreen),
                canExecute: SetWorkspace);

            GoToPart = NavigationCommand.Create<Part, PartViewModel>(
                hostScreen: HostScreen,
                viewModelFactory: CreatePartViewModel,
                canExecute: SetWorkspace);

            GoToAbout = NavigationCommand.Create(
                hostScreen: HostScreen,
                viewModelFactory: () => new AboutViewModel(HostScreen));

            BuildLibrary = ReactiveCommand.CreateFromTask(libraryBuilder.Build, SetWorkspace);

            LoadParts = ReactiveCommand.CreateFromTask(ExecuteLoadParts, SetWorkspace);

            SelectWorkspace = new Interaction<string?, string?>();
        }

        public ReactiveCommand<Unit, Unit> BuildLibrary { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> GoToAbout { get; }

        public ReactiveCommand<Part, IRoutableViewModel> GoToPart { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> GoToSettings { get; }

        public ReactiveCommand<Unit, bool> SetWorkspace { get; }

        public Interaction<string?, string?> SelectWorkspace { get; }

        public ReactiveCommand<Unit, Unit> LoadParts { get; }

        [DependsOn(nameof(Parts))]
        public IReadOnlyList<ColumnInfo> PartColumns => GetColumnInfos(Parts);

        public IReadOnlyList<Part> Parts
        {
            get => _parts;
            set => this.RaiseAndSetIfChanged(ref _parts, value);
        }

        private static IReadOnlyList<ColumnInfo> GetColumnInfos(IEnumerable<Part> parts)
        {
            var columnInfos = new List<ColumnInfo>()
            {
                new ColumnInfo(nameof(Part.Library)),
                new ColumnInfo(nameof(Part.Reference)),
                new ColumnInfo(nameof(Part.Value)),
                new ColumnInfo(nameof(Part.Symbol)),
                new ColumnInfo(nameof(Part.Footprint)),
                new ColumnInfo(nameof(Part.Description)),
                new ColumnInfo(nameof(Part.Keywords)),
                new ColumnInfo(nameof(Part.Datasheet)),
            };

            if (parts != null)
            {
                var customFieldColumnInfos = parts
                    .SelectMany(part => part.CustomFields)
                    .Where(kv => !string.IsNullOrWhiteSpace(kv.Value))
                    .OrderBy(kv => kv.Key)
                    .Select(kv => kv.Key)
                    .Distinct()
                    .Select(field => new ColumnInfo(field, $"{nameof(Part.CustomFields)}[{field}]"));

                columnInfos.AddRange(customFieldColumnInfos);
            }

            return columnInfos;
        }

        private async Task ExecuteLoadParts()
        {
            var parts = await _partRepository.GetPartsAsync().ConfigureAwait(false);
            var customFields = parts
                .SelectMany(part => part.CustomFields.Keys)
                .Distinct()
                .ToArray();

            foreach (var part in parts)
            {
                part.CustomFields = customFields.ToDictionary(
                    field => field,
                    field => part.CustomFields.GetValueOrDefault(field, string.Empty));
            }

            Parts = parts
                .OrderBy(part => part.Library)
                .ToArray();
        }

        private async Task<PartViewModel> CreatePartViewModel(Part? part = null)
        {
            if (part?.Id is null)
            {
                part = new Part();
            }
            else
            {
                part = await _partRepository.GetPartAsync(part.Id)
                    .ConfigureAwait(true);
            }

            return new PartViewModel(
                     hostScreen: HostScreen,
                     part: part);
        }

        private async Task<bool> ExecuteSetWorkspace()
        {
            var appSettings = await _settingsProvider.GetAppSettings().ConfigureAwait(true);

            var workspace = appSettings.Workspace;
            var selection = await SelectWorkspace.Handle(workspace).Catch(Observable.Return(workspace));

            appSettings.Workspace = selection;
            await _settingsProvider.UpdateAppSettings(appSettings).ConfigureAwait(true);

            var hasWorkspace = !string.IsNullOrEmpty(appSettings.Workspace);

            return hasWorkspace;
        }
    }
}