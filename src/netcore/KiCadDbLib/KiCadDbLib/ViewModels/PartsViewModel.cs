using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using DynamicData.Alias;
using KiCadDbLib.Models;
using KiCadDbLib.Navigation;
using KiCadDbLib.Views;
using ReactiveUI;
using SharpDX.Direct2D1.Effects;

namespace KiCadDbLib.ViewModels
{
    public class PartsViewModel : ViewModelBase, IRoutableViewModel
    {
        private IEnumerable<Part> _parts;
        private ObservableAsPropertyHelper<IEnumerable<ColumnInfo>> _partColumns;


        public PartsViewModel(IScreen hostScreen)
        {
            HostScreen = hostScreen ?? throw new ArgumentNullException(nameof(hostScreen));
            UrlPathSegment = Guid.NewGuid().ToString().Substring(0, 5);

            GoToSettings = NavigationCommand.Create(HostScreen, () => new SettingsViewModel(HostScreen, new Services.SettingsService()));
            _partColumns = this.WhenAnyValue(vm => vm.Parts)
                .Select(parts => GetColumnInfos(parts))
                .ToProperty(this, vm => vm.PartColumns);
            Parts = MockParts();            
        }
                
        private static IEnumerable<ColumnInfo> GetColumnInfos(IEnumerable<Part> parts)
        {
            List<ColumnInfo> columnInfos = new List<ColumnInfo>()
            {
                new ColumnInfo(nameof(Part.Id)),
                new ColumnInfo(nameof(Part.Library)),
                new ColumnInfo(nameof(Part.Reference)),
            };

            if(parts != null)
            {
                var customFieldColumnInfos = parts
                    .SelectMany(part => part.CustomFields.Keys)
                    .Distinct()
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

        /// <summary>
        /// Gets the <see cref="IScreen"/> that owns the routable view model.
        /// </summary>
        public IScreen HostScreen { get; }

        /// <summary>
        /// Gets the unique identifier for the routable view model.
        /// </summary>
        public string UrlPathSegment { get; }

        public IEnumerable<Part> Parts
        {
            get => _parts;
            set => this.RaiseAndSetIfChanged(ref _parts, value);
        }
        public IEnumerable<ColumnInfo> PartColumns => _partColumns.Value;

        public ReactiveCommand<Unit, IRoutableViewModel> GoToSettings { get; }
    }
}