using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls.Templates;
using Avalonia.Media;
using Avalonia.Rendering.SceneGraph;
using KiCadDbLib.Models;
using KiCadDbLib.Navigation;
using KiCadDbLib.ReactiveForms;
using KiCadDbLib.ReactiveForms.Validation;
using KiCadDbLib.Services;
using KiCadDbLib.Services.KiCad;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReactiveUI;

namespace KiCadDbLib.ViewModels
{
    public class PartViewModel : RoutableViewModelBase
    {
        private readonly PartsService _partsService;
        private readonly SettingsService _settingsService;
        private string _id;
        private Part _part;
        private ObservableAsPropertyHelper<FormGroup> _partFormProperty;
        public PartViewModel(IScreen hostScreen, SettingsService settingsService, PartsService partsService, Part part)
            : base(hostScreen)
        {
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            _partsService = partsService ?? throw new ArgumentNullException(nameof(partsService));
            _part = part ?? new Part();
            Id = _part.Id;

            GoBack = ReactiveCommand.CreateFromTask(ExecuteGoBackAsync);

            var canCloneOrDelete = this.WhenAnyValue(vm => vm.Id)
                .Select(id => !string.IsNullOrEmpty(Id));
            Clone = ReactiveCommand.Create(ExecuteClone, canCloneOrDelete);
            Delete = ReactiveCommand.CreateFromTask(ExecuteDeleteAsync, canCloneOrDelete);
            Save = ReactiveCommand.CreateFromTask(ExecuteSaveAsync);
            DiscardChangesConfirmation = new Interaction<Unit, bool>();
            DeletePartConfirmation = new Interaction<Unit, bool>();
        }
        public Func<string, CancellationToken, Task<IEnumerable<object>>> AsyncAvailableFootprintsPopulator => GetAvailableFootrpintsAsync;

        public Func<string, CancellationToken, Task<IEnumerable<object>>> AsyncAvailableSymbolsPopulator => GetAvailableSymbolsAsync;

        public ReactiveCommand<Unit, Unit> Clone { get; }

        public ReactiveCommand<Unit, Unit> Delete { get; }

        public Interaction<Unit, bool> DeletePartConfirmation { get; }

        public Interaction<Unit, bool> DiscardChangesConfirmation { get; }

        public ReactiveCommand<Unit, Unit> GoBack { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> GoToSettings { get; }

        public string Id
        {
            get => _id;
            set => this.RaiseAndSetIfChanged(ref _id, value);
        }
        public FormGroup PartForm => _partFormProperty?.Value;


        public ReactiveCommand<Unit, Unit> Save { get; }


        protected override void WhenActivated(CompositeDisposable disposables)
        {
            base.WhenActivated(disposables);

            IObservable<Settings> settingsObservable = _settingsService.GetSettingsAsync().ToObservable();
            IObservable<(LibraryItemInfo[] Symbols, LibraryItemInfo[] Footprints)> kicadObservable = settingsObservable.SelectMany(settings =>
            {
                IObservable<LibraryItemInfo[]> symbolsObservable = _partsService
                    .GetSymbolsAsync()
                    .ToObservable();

                IObservable<LibraryItemInfo[]> footprintsObservable = _partsService
                   .GetFootprintsAsync()
                   .ToObservable();

                return symbolsObservable
                    .ForkJoin(
                       footprintsObservable, 
                        (symbols, footprints) => (Symbols: symbols, Footprints: footprints));
            });

            _partFormProperty = settingsObservable
                .ForkJoin(kicadObservable, (settings, kicad) => (Settings: settings, KiCad: kicad))      
                .Select(joined => GetPartForm(joined.Settings, joined.KiCad.Symbols, joined.KiCad.Footprints, _part))
                .ToProperty(this, nameof(PartForm))
                .DisposeWith(disposables);
        }
        private static FormGroup GetPartForm(Settings settings, IEnumerable<LibraryItemInfo> symbols, IEnumerable<LibraryItemInfo> footprints, Part part)
        {

            FormGroup customFields = new FormGroup()
            {
                Label = "Custom fields",
            };

            foreach (string customField in settings.CustomFields)
            {
                FormControl formControl = new FormControl(part.CustomFields.TryGetValue(customField, out string temp) ? temp : string.Empty)
                {
                    Label = customField,
                };

                customFields.Add(customField, formControl);
            }

            FormGroup basicFields = new FormGroup("Basic fields");

            basicFields.Add(nameof(Part.Library), new AutoCompleteFormControl(part.Library)
            {
                Label = nameof(Part.Library),
                Validator = Validators.Compose(
                        Validators.Required,
                        Validators.Pattern(new Regex(@"^[a-zA-Z0-9_\-\. ]*$"))),
                Items = new [] {"1", "2", "3"},

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
                Items = symbols
                    .Select(x => new LibraryItemInfo() { Name = x.Name, Library = Path.GetFileNameWithoutExtension(x.Library) })
                    .Select(x => x.ToString()),
            });

            basicFields.Add(nameof(Part.Footprint), new AutoCompleteFormControl(part.Footprint)
            {
                Label = nameof(Part.Footprint),
                Items = footprints
                    .Select(x => new LibraryItemInfo() { Name = x.Name, Library = Path.GetFileNameWithoutExtension(x.Library) })
                    .Select(x => x.ToString()),
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



            FormGroup form = new FormGroup();
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
                await _partsService.DeleteAsync(_part.Id);
                await HostScreen.Router.NavigateBack.Execute();
            }
        }

        private async Task ExecuteGoBackAsync()
        {
            if (!PartForm.IsDirty
                || await DiscardChangesConfirmation.Handle(default).Catch(Observable.Return(true)))
            {
                await HostScreen.Router.NavigateBack.Execute();
            }
        }

        private async Task ExecuteSaveAsync()
        {
            if (!PartForm.Validate())
            {
                return;
            }

            JObject part = PartForm.Controls
                .Select(item => item.Control)
                .First()
                .GetValue() as JObject;

            part[nameof(Part.CustomFields)] = PartForm.Controls
                .Select(item => item.Control)
                .Skip(1)
                .First()
                .GetValue() as JObject;

            _part = part.ToObject<Part>();
            _part.Id = Id;

            await _partsService.AddOrUpdateAsync(_part);
            await HostScreen.Router.NavigateBack.Execute();
        }

        private async Task<IEnumerable<object>> GetAvailableFootrpintsAsync(string searchText, CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
            {
                return new[] { "Device:C", "Device:R", "Device:L" }
                    .Where(symbol => symbol.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                    .Cast<object>();
            }, cancellationToken);
        }

        private async Task<IEnumerable<object>> GetAvailableSymbolsAsync(string searchText, CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
            {
                return new[] { "Device:C", "Device:R", "Device:L" }
                    .Where(symbol => symbol.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                    .Cast<object>();
            }, cancellationToken);
        }

    }
}