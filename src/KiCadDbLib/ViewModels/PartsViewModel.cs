using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
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
        private readonly IPartRepository _partRepository;
        private ObservableAsPropertyHelper<IEnumerable<ColumnInfo>> _partColumnsProperty;

        public PartsViewModel(IScreen hostScreen)
            : base(hostScreen)
        {
            _partRepository = Locator.Current.GetRequiredService<IPartRepository>()!;
            var libraryBuilder = Locator.Current.GetRequiredService<ILibraryBuilder>()!;

            GoToSettings = NavigationCommand.Create(
                hostScreen: HostScreen,
                viewModelFactory: () => new SettingsViewModel(hostScreen: HostScreen));

            GoToPart = NavigationCommand.Create<Part, PartViewModel>(
                hostScreen: HostScreen,
                viewModelFactory: CreatePartViewModel);

            GoToAbout = NavigationCommand.Create(
                hostScreen: HostScreen,
                viewModelFactory: () => new AboutViewModel(HostScreen));

            BuildLibrary = ReactiveCommand.CreateFromTask(libraryBuilder.Build);

            LoadParts = ReactiveCommand.CreateFromTask(async () =>
            {
                return (await _partRepository.GetPartsAsync().ConfigureAwait(false))
                    .OrderBy(part => part.Library)
                    .ToArray();
            });
        }

        public ReactiveCommand<Unit, Unit> BuildLibrary { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> GoToAbout { get; }

        public ReactiveCommand<Part, IRoutableViewModel> GoToPart { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> GoToSettings { get; }

        public ReactiveCommand<Unit, Part[]> LoadParts { get; }

        public IEnumerable<ColumnInfo> PartColumns => _partColumnsProperty?.Value;

        /// <inheritdoc/>
        protected override void WhenActivated(CompositeDisposable disposables)
        {
            base.WhenActivated(disposables);

            _partColumnsProperty = LoadParts
                .Select(parts => GetColumnInfos(parts))
                .ToProperty(this, nameof(PartColumns))
                .DisposeWith(disposables);
        }

        private static IEnumerable<ColumnInfo> GetColumnInfos(IEnumerable<Part> parts)
        {
            List<ColumnInfo> columnInfos = new List<ColumnInfo>()
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
                    .SelectMany(part => part.CustomFields.Keys)
                    .Distinct()
                    .OrderBy(x => x)
                    .Select(field => new ColumnInfo(field, $"{nameof(Part.CustomFields)}[{field}]"));

                columnInfos.AddRange(customFieldColumnInfos);
            }

            return columnInfos;
        }

        private async Task<PartViewModel> CreatePartViewModel(Part part = null)
        {
            if (part is null)
            {
                part = new Part();
            }
            else
            {
                part = await _partRepository.GetPartAsync(part.Id)
                    .ConfigureAwait(false);
            }

            return new PartViewModel(
                     hostScreen: HostScreen,
                     part: part);
        }
    }
}