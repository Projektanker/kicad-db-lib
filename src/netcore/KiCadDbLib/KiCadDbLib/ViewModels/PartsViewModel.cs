using System;
using System.Collections.Generic;
using System.Data;
using System.Reactive;
using System.Reactive.Linq;
using DynamicData.Alias;
using KiCadDbLib.Models;
using KiCadDbLib.Navigation;
using KiCadDbLib.Views;
using ReactiveUI;

namespace KiCadDbLib.ViewModels
{
    public class PartsViewModel : ViewModelBase, IRoutableViewModel
    {
        private IEnumerable<Part> _parts;

        public PartsViewModel(IScreen hostScreen)
        {
            HostScreen = hostScreen ?? throw new ArgumentNullException(nameof(hostScreen));
            UrlPathSegment = Guid.NewGuid().ToString().Substring(0, 5);

            GoToSettings = NavigationCommand.Create(HostScreen, () => new SettingsViewModel(HostScreen, new Services.SettingsService()));
            Parts = MockParts();
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

        public IEnumerable<Part> Parts { 
            get => _parts; 
            set => this.RaiseAndSetIfChanged(ref _parts, value); }

        public ReactiveCommand<Unit, IRoutableViewModel> GoToSettings { get; }
    }
}