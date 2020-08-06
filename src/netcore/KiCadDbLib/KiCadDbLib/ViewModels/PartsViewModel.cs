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
            HashSet<ColumnInfo> columnInfos = new HashSet<ColumnInfo>();
            foreach (var part in parts ?? Enumerable.Empty<Part>())
            {
                foreach (var item in part.CustomFields.Keys)
                {
                    columnInfos.Add(new ColumnInfo(item, $"CustomFields[{item}]"));
                }
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