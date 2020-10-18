using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Controls.Notifications;
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

        public PartsViewModel(IScreen hostScreen)
            : base(hostScreen)
        {
            _partsService = Locator.Current.GetService<PartsService>();
            GoToSettings = NavigationCommand.Create(
                hostScreen: HostScreen,
                viewModelFactory: () => new SettingsViewModel(hostScreen: HostScreen));

            GoToPart = NavigationCommand.Create<Part, PartViewModel>(hostScreen: HostScreen,
                viewModelFactory: CreatePartViewModel);

            GoToAbout = NavigationCommand.Create(hostScreen: HostScreen,
                viewModelFactory: () => new AboutViewModel(HostScreen));

            BuildLibrary = ReactiveCommand.CreateFromTask(_partsService.Build);

            LoadParts = ReactiveCommand.CreateFromTask(async () => (await _partsService.GetPartsAsync()).OrderBy(part => part.Library).ToArray());
        }

        private PartViewModel CreatePartViewModel(Part part = null)
        {
            return new PartViewModel(
                     hostScreen: HostScreen,
                     part: part ?? new Part());
        }

        public ReactiveCommand<Part, IRoutableViewModel> GoToPart { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> GoToSettings { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> GoToAbout { get; }

        public ReactiveCommand<Unit, Unit> BuildLibrary { get; }
        public ReactiveCommand<Unit, Part[]> LoadParts { get; }

        public IEnumerable<ColumnInfo> PartColumns => _partColumnsProperty?.Value;

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
                // new ColumnInfo(nameof(Part.Id)),
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