using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using KiCadDbLib.Models;
using KiCadDbLib.Navigation;
using KiCadDbLib.Services;
using ReactiveUI;

namespace KiCadDbLib.ViewModels
{
    public class PartViewModel : RoutableViewModelBase
    {
        private readonly Part _part;
        private readonly PartsService _partsService;
        private readonly SettingsService _settingsService;
        private ObservableAsPropertyHelper<IEnumerable<CustomField>> _customFieldsProperty;
        private string _datasheet;
        private string _description;
        private string _footprint;
        private string _id;
        private string _keywords;
        private string _library;
        private string _reference;
        private string _symbol;
        private string _value;
        public PartViewModel(IScreen hostScreen, SettingsService settingsService, PartsService partsService, Part part)
            : base(hostScreen)
        {
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            _partsService = partsService ?? throw new ArgumentNullException(nameof(partsService));
            _part = part ?? new Part();

            Id = _part.Id;
            Library = _part.Library;
            Reference = _part.Reference;
            Value = _part.Value;
            Symbol = _part.Symbol;
            Footprint = _part.Footprint;
            Description = _part.Description;
            Keywords = _part.Keywords;
            Datasheet = _part.Datasheet;

            GoBack = ReactiveCommand.CreateFromTask(ExecuteGoBackAsync);

            var canCloneOrDelete = this.WhenAnyValue(vm => vm.Id, id => !string.IsNullOrEmpty(id));
            Clone = ReactiveCommand.Create(ExecuteClone, canCloneOrDelete);
            Delete = ReactiveCommand.CreateFromTask(ExecuteDeleteAsync, canCloneOrDelete);
            Save = ReactiveCommand.CreateFromTask(ExecuteSaveAsync);
            DiscardChangesConfirmation = new Interaction<Unit, bool>();
            DeletePartConfirmation = new Interaction<Unit, bool>();
        }

        public ReactiveCommand<Unit, Unit> Clone { get; }

        public IEnumerable<CustomField> CustomFields => _customFieldsProperty?.Value;

        /// <summary>
        /// Gets or sets the <see cref="Part.Datasheet"/>
        /// </summary>
        public string Datasheet
        {
            get => _datasheet;
            set => this.RaiseAndSetIfChanged(ref _datasheet, value);
        }

        public ReactiveCommand<Unit, Unit> Delete { get; }

        /// <summary>
        /// Gets or sets the <see cref="Part.Description"/>
        /// </summary>
        public string Description
        {
            get => _description;
            set => this.RaiseAndSetIfChanged(ref _description, value);
        }

        public Interaction<Unit, bool> DiscardChangesConfirmation { get; }
        public Interaction<Unit, bool> DeletePartConfirmation { get; }

        /// <summary>
        /// Gets or sets the <see cref="Part.Footprint"/>
        /// </summary>
        public string Footprint
        {
            get => _footprint;
            set => this.RaiseAndSetIfChanged(ref _footprint, value);
        }

        public ReactiveCommand<Unit, Unit> GoBack { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> GoToSettings { get; }

        /// <summary>
        /// Gets the <see cref="Part.Id"/>
        /// </summary>
        public string Id
        {
            get => _id;
            set => this.RaiseAndSetIfChanged(ref _id, value);
        }

        /// <summary>
        /// Gets or sets the <see cref="Part.Keywords"/>
        /// </summary>
        public string Keywords
        {
            get => _keywords;
            set => this.RaiseAndSetIfChanged(ref _keywords, value);
        }

        /// <summary>
        /// Gets the <see cref="Part.Library"/>
        /// </summary>
        public string Library
        {
            get => _library;
            set => this.RaiseAndSetIfChanged(ref _library, value);
        }

        /// <summary>
        /// Gets or sets the <see cref="Part.Reference"/>
        /// </summary>
        public string Reference
        {
            get => _reference;
            set => this.RaiseAndSetIfChanged(ref _reference, value);
        }

        public ReactiveCommand<Unit, Unit> Save { get; }

        /// <summary>
        /// Gets or sets the <see cref="Part.Symbol"/>
        /// </summary>
        public string Symbol
        {
            get => _symbol;
            set => this.RaiseAndSetIfChanged(ref _symbol, value);
        }

        /// <summary>
        /// Gets or sets the <see cref="Part.Value"/>
        /// </summary>
        public string Value
        {
            get => _value;
            set => this.RaiseAndSetIfChanged(ref _value, value);
        }

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
                .Select(cf => new CustomField(name: cf, value: part.CustomFields.TryGetValue(cf, out string value) ? value : string.Empty))
                .ToArray();
        }

        private void ExecuteClone()
        {
            Id = null;
        }

        private async Task ExecuteDeleteAsync()
        {
            if (await DeletePartConfirmation.Handle(default).Catch(Observable.Return(true)))
            {
                await _partsService.DeleteAsync(Id);
                await HostScreen.Router.NavigateBack.Execute();
            }
        }
        private async Task ExecuteGoBackAsync()
        {
            if (!IsDirty()
                || await DiscardChangesConfirmation.Handle(default).Catch(Observable.Return(true)))
            {
                await HostScreen.Router.NavigateBack.Execute();
            }
        }

        private async Task ExecuteSaveAsync()
        {
            _part.Id = Id;
            _part.Library = Library;
            _part.Reference = Reference;
            _part.Value = Value;
            _part.Symbol = Symbol;
            _part.Footprint = Footprint;
            _part.Description = Description;
            _part.Keywords = Keywords;
            _part.Datasheet = Datasheet;

            foreach (CustomField customField in CustomFields)
            {
                _part.CustomFields[customField.Name] = customField.Value;
            }

            await _partsService.AddOrUpdateAsync(_part);
            await HostScreen.Router.NavigateBack.Execute();
        }

        private bool IsDirty()
        {
            return Id != _part.Id
                || Library != _part.Library
                || Reference != _part.Reference
                || Value != _part.Value
                || Symbol != _part.Symbol
                || Footprint != _part.Footprint
                || Description != _part.Description
                || Keywords != _part.Keywords
                || Datasheet != _part.Datasheet
                || CustomFields.Any(cf =>
                {
                    string value = _part.CustomFields.TryGetValue(cf.Value, out string temp) ? temp : string.Empty;
                    return cf.Value != value;
                });
        }
    }
}