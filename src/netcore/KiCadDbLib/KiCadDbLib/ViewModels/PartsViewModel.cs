using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData.Alias;
using KiCadDbLib.Models;
using KiCadDbLib.Navigation;
using KiCadDbLib.Views;
using ReactiveUI;
using SharpDX.Direct2D1.Effects;

namespace KiCadDbLib.ViewModels
{
    public class PartsViewModel : RoutableViewModelBase
    {
        private ObservableAsPropertyHelper<IEnumerable<ColumnInfo>> _partColumns;
        private IEnumerable<Part> _parts;
        public PartsViewModel(IScreen hostScreen)
            : base(hostScreen)
        {
            GoToSettings = NavigationCommand.Create(HostScreen, () => new SettingsViewModel(HostScreen, new Services.SettingsService()));
            
            Parts = MockParts();            
        }

        protected override void WhenActivated(CompositeDisposable disposables)
        {
            base.WhenActivated(disposables);

            _partColumns = this.WhenAnyValue(vm => vm.Parts)
                .Select(parts => GetColumnInfos(parts))
                .ToProperty(this, vm => vm.PartColumns);
        }

        public ReactiveCommand<Unit, IRoutableViewModel> GoToSettings { get; }

        public IEnumerable<ColumnInfo> PartColumns => _partColumns?.Value;

        public IEnumerable<Part> Parts
        {
            get => _parts;
            set => this.RaiseAndSetIfChanged(ref _parts, value);
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

        private IEnumerable<Part> MockParts()
        {
            yield return new Part()
            {
                Id = "0001",
                Library = "_R_0603",
                Reference = "R",
                CustomFields =
                {
                    { "MF", "Wuerth" },
                }
            };

            yield return new Part()
            {
                Id = "0002",
                Library = "_R_0805",
                Reference = "R",
                CustomFields =
                {
                    { "MF", "Wuerth" },
                }
            };

            yield return new Part()
            {
                Id = "0003",
                Library = "_C_0603",
                Reference = "R",
                CustomFields =
                {
                    { "MF", "Wuerth" },
                }
            };
        }
    }
}