using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using KiCadDbLib.Models;
using KiCadDbLib.Navigation;
using KiCadDbLib.Services;
using ReactiveUI;

namespace KiCadDbLib.ViewModels
{
    public class PartViewModel : RoutableViewModelBase
    {
        private readonly Part _part;
        private readonly SettingsService _settingsService;
        private ObservableAsPropertyHelper<IEnumerable<CustomField>> _customFieldsProperty;
        public PartViewModel(IScreen hostScreen, SettingsService settingsService, Part part)
            : base(hostScreen)
        {
            _settingsService = settingsService;
            _part = part;

            Id = _part.Id;
            Library = _part.Library;
            Reference = _part.Reference;
            Value = _part.Value;
            Symbol = _part.Symbol;
            Footprint = _part.Footprint;
            Desciption = _part.Description;
            Keywords = _part.Keywords;
            Datasheet = _part.Datasheet;
        }

        public IEnumerable<CustomField> CustomFields => _customFieldsProperty?.Value;

        /// <summary>
        /// Gets or sets the <see cref="Part.Datasheet"/>
        /// </summary>
        public string Datasheet { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Part.Description"/>
        /// </summary>
        public string Desciption { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Part.Footprint"/>
        /// </summary>
        public string Footprint { get; set; }

        public ReactiveCommand<Unit, IRoutableViewModel> GoToSettings { get; }

        /// <summary>
        /// Gets the <see cref="Part.Id"/>
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Part.Keywords"/>
        /// </summary>
        public string Keywords { get; set; }

        /// <summary>
        /// Gets the <see cref="Part.Library"/>
        /// </summary>
        public string Library { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Part.Reference"/>
        /// </summary>
        public string Reference { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Part.Symbol"/>
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Part.Value"/>
        /// </summary>
        public string Value { get; set; }

        protected override void WhenActivated(CompositeDisposable disposables)
        {
            base.WhenActivated(disposables);

            _customFieldsProperty = _settingsService.GetSettingsAsync().ToObservable()
                .Select(settings => GetCustomFields(settings, _part))
                .ToProperty(this, nameof(CustomFields))
                .DisposeWith(disposables);
        }
        private static IEnumerable<CustomField> GetCustomFields(Settings settings, Part part)
        {
            return settings.CustomFields
                .Select(cf => new CustomField(name: cf, value: part.CustomFields.TryGetValue(cf, out string value) ? value : string.Empty));
        }

    }
}