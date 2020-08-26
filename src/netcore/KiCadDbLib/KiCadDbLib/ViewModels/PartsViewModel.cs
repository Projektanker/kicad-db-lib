using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
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
        private readonly PartsService _partsService;
        private ObservableAsPropertyHelper<IEnumerable<ColumnInfo>> _partColumnsProperty;
        private ObservableAsPropertyHelper<IEnumerable<Part>> _partsProperty;

        public PartsViewModel(IScreen hostScreen, PartsService partsService)
            : base(hostScreen)
        {
            _partsService = partsService;
            GoToSettings = NavigationCommand.Create(
                hostScreen: HostScreen,
                viewModelFactory: () => new SettingsViewModel(
                    hostScreen: HostScreen,
                    settingsService: Locator.Current.GetService<SettingsService>(),
                    partsService: new PartsService(Locator.Current.GetService<SettingsService>())));

            GoToPart = NavigationCommand.Create<Part, PartViewModel>(hostScreen: HostScreen,
                viewModelFactory: CreatePartViewModel);
        }

        private PartViewModel CreatePartViewModel(Part part = null)
        {
            return new PartViewModel(
                     hostScreen: HostScreen,
                     settingsService: Locator.Current.GetService<SettingsService>(),
                     partsService: Locator.Current.GetService<PartsService>(),
                     part: part ?? new Part());
        }

        public ReactiveCommand<Part, IRoutableViewModel> GoToPart { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> GoToSettings { get; }

        public IEnumerable<ColumnInfo> PartColumns => _partColumnsProperty?.Value;

        public IEnumerable<Part> Parts => _partsProperty?.Value;

        protected override void WhenActivated(CompositeDisposable disposables)
        {
            base.WhenActivated(disposables);

            _partColumnsProperty = this.WhenAnyValue(vm => vm.Parts)
                .Select(parts => GetColumnInfos(parts))
                .ToProperty(this, vm => vm.PartColumns)
                .DisposeWith(disposables);

            var partsObservable = _partsService.GetPartsAsync().ToObservable();
            _partsProperty = partsObservable
                .Cast<IEnumerable<Part>>()
                .ToProperty(this, nameof(Parts))
                .DisposeWith(disposables);
        }

        private static IEnumerable<ColumnInfo> GetColumnInfos(IEnumerable<Part> parts)
        {
            List<ColumnInfo> columnInfos = new List<ColumnInfo>()
            {
                new ColumnInfo(nameof(Part.Id)),
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
    }
}