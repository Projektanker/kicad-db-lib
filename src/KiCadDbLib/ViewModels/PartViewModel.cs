using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using KiCadDbLib.Models;
using KiCadDbLib.ReactiveForms;
using KiCadDbLib.ReactiveForms.Validation;
using KiCadDbLib.Services;
using Newtonsoft.Json.Linq;
using ReactiveUI;
using Splat;

namespace KiCadDbLib.ViewModels
{
    public class PartViewModel : RoutableViewModelBase
    {
        private readonly ILibraryBuilder _libaryBuilder;
        private readonly ILibraryReader _libaryReader;
        private readonly IPartRepository _partRepository;
        private readonly ISettingsProvider _settingsService;
        private string? _id;
        private Part _part;
        private ObservableAsPropertyHelper<FormGroup>? _partFormProperty;

        public PartViewModel(
            IScreen hostScreen,
            Part part)
            : base(hostScreen)
        {
            _part = part ?? new Part();
            Id = _part.Id;

            _libaryBuilder = Locator.Current.GetRequiredService<ILibraryBuilder>();
            _libaryReader = Locator.Current.GetRequiredService<ILibraryReader>();
            _partRepository = Locator.Current.GetRequiredService<IPartRepository>();
            _settingsService = Locator.Current.GetRequiredService<ISettingsProvider>();

            GoBack = ReactiveCommand.CreateFromTask(ExecuteGoBackAsync);

            var canCloneOrDelete = this.WhenAnyValue(vm => vm.Id)
                .Select(id => !string.IsNullOrEmpty(id));
            Clone = ReactiveCommand.Create(ExecuteClone, canCloneOrDelete);
            Delete = ReactiveCommand.CreateFromTask(ExecuteDeleteAsync, canCloneOrDelete);
            Save = ReactiveCommand.CreateFromTask(ExecuteSaveAsync);
            DiscardChangesConfirmation = new Interaction<Unit, bool>();
            DeletePartConfirmation = new Interaction<Unit, bool>();
        }

        public ReactiveCommand<Unit, Unit> Clone { get; }

        public ReactiveCommand<Unit, Unit> Delete { get; }

        public Interaction<Unit, bool> DeletePartConfirmation { get; }

        public Interaction<Unit, bool> DiscardChangesConfirmation { get; }

        public ReactiveCommand<Unit, Unit> GoBack { get; }

        public string? Id
        {
            get => _id;
            set => this.RaiseAndSetIfChanged(ref _id, value);
        }

        public FormGroup? PartForm => _partFormProperty?.Value;

        public ReactiveCommand<Unit, Unit> Save { get; }

        protected override void WhenActivated(CompositeDisposable disposables)
        {
            base.WhenActivated(disposables);

            var settingsObservable = _settingsService.GetSettingsAsync().ToObservable();
            var symbolsObservable = _libaryReader
                .GetSymbolsAsync()
                .ToObservable();

            var footprintsObservable = _libaryReader
                .GetFootprintsAsync()
                .ToObservable();

            var librariesObservable = _partRepository
                .GetLibrariesAsync()
                .ToObservable();

            var kicadObservable = symbolsObservable
                .ForkJoin(
                    second: footprintsObservable,
                    resultSelector: (symbols, footprints) => (Symbols: symbols, Footprints: footprints))
                .ForkJoin(
                    second: librariesObservable,
                    resultSelector: (sf, libraries) => (sf.Symbols, sf.Footprints, Libraries: libraries));

            _partFormProperty = settingsObservable
                .ForkJoin(kicadObservable, (settings, kicad) => (Settings: settings, KiCad: kicad))
                .Select(joined => GetPartForm(joined.Settings, joined.KiCad.Symbols, joined.KiCad.Footprints, joined.KiCad.Libraries, _part))
                .ToProperty(this, nameof(PartForm))
                .DisposeWith(disposables);
        }

        private static FormGroup GetPartForm(Settings settings, IEnumerable<string> symbols, IEnumerable<string> footprints, IEnumerable<string> libraries, Part part)
        {
            var customFields = new FormGroup()
            {
                Label = "Custom fields",
            };

            foreach (string customField in settings.CustomFields)
            {
                var formControl = new FormControl(part.CustomFields.TryGetValue(customField, out var temp) ? temp : string.Empty)
                {
                    Label = customField,
                };

                customFields.Add(customField, formControl);
            }

            var basicFields = new FormGroup("Basic fields");

            basicFields.Add(nameof(Part.Library), new AutoCompleteFormControl(part.Library)
            {
                Label = nameof(Part.Library),
                Validator = Validators.Compose(
                        Validators.Required,
                        Validators.Pattern(new Regex(@"^[a-zA-Z0-9_\-\. ]*$"))),
                Items = libraries,
            });

            basicFields.Add(nameof(Part.Reference), new FormControl(part.Reference)
            {
                Label = nameof(Part.Reference),
                Validator = Validators.Compose(
                        Validators.Required,
                        Validators.Pattern(new Regex(@"^[^ \\/:]*$"))),
            });

            basicFields.Add(nameof(Part.Value), new FormControl(part.Value)
            {
                Label = nameof(Part.Value),
                Validator = Validators.Compose(
                        Validators.Required,
                        Validators.Pattern(new Regex(@"^[^ \\/:]*$"))),
            });

            basicFields.Add(nameof(Part.Symbol), new AutoCompleteFormControl(part.Symbol)
            {
                Label = nameof(Part.Symbol),
                Validator = Validators.Required,
                Items = symbols,
            });

            basicFields.Add(nameof(Part.Footprint), new AutoCompleteFormControl(part.Footprint)
            {
                Label = nameof(Part.Footprint),
                Items = footprints,
            });

            basicFields.Add(nameof(Part.Description), new FormControl(part.Description)
            {
                Label = nameof(Part.Description),
            });

            basicFields.Add(nameof(Part.Keywords), new FormControl(part.Keywords)
            {
                Label = nameof(Part.Keywords),
            });

            basicFields.Add(nameof(Part.Datasheet), new FormControl(part.Datasheet)
            {
                Label = nameof(Part.Datasheet),
            });

            var form = new FormGroup();
            form.Add("BasicFields", basicFields);
            form.Add("CustomFields", customFields);

            return form;
        }

        private void ExecuteClone()
        {
            Id = null;
        }

        private async Task ExecuteDeleteAsync()
        {
            if (await DeletePartConfirmation.Handle(default).Catch(Observable.Return(true)))
            {
                await _partRepository.DeleteAsync(_part.Id!).ConfigureAwait(false);
                await HostScreen.Router.NavigateBack.Execute();
            }
        }

        private async Task ExecuteGoBackAsync()
        {
            if (!(PartForm?.IsDirty ?? false)
                || await DiscardChangesConfirmation.Handle(default).Catch(Observable.Return(true)))
            {
                await HostScreen.Router.NavigateBack.Execute();
            }
        }

        private async Task ExecuteSaveAsync()
        {
            if (!PartForm!.Validate())
            {
                throw new ValidationException("Input is invalid.");
            }

            var part = PartForm.Controls
                .Select(item => item.Control)
                .First()
                .GetValue() as JObject;

            part![nameof(Part.CustomFields)] = PartForm.Controls
                .Select(item => item.Control)
                .Skip(1)
                .First()
                .GetValue() as JObject;

            _part = part.ToObject<Part>() ?? new Part();
            _part.Id = Id!;

            await HostScreen.Router.NavigateBack.Execute();
            await _partRepository.AddOrUpdateAsync(_part).ConfigureAwait(true);
            await _libaryBuilder.Build().ConfigureAwait(true);
        }
    }
}