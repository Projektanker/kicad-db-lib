using System;
using System.Reactive;
using KiCadDbLib.Views;
using ReactiveUI;

namespace KiCadDbLib.ViewModels
{
    public class PartsViewModel : ViewModelBase, IRoutableViewModel
    {
        public PartsViewModel(IScreen hostScreen)
        {
            HostScreen = hostScreen ?? throw new ArgumentNullException(nameof(hostScreen));
            UrlPathSegment = Guid.NewGuid().ToString().Substring(0, 5);

            GoToSettings = ReactiveCommand.CreateFromObservable(
                () => HostScreen.Router.Navigate.Execute(new SettingsViewModel(HostScreen, new Services.SettingsService())));
        }

        /// <summary>
        /// Gets the <see cref="IScreen"/> that owns the routable view model.
        /// </summary>
        public IScreen HostScreen { get; }

        /// <summary>
        /// Gets the unique identifier for the routable view model.
        /// </summary>
        public string UrlPathSegment { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> GoToSettings { get; }
    }
}