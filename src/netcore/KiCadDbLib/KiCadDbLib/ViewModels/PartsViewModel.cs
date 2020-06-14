using System;
using ReactiveUI;

namespace KiCadDbLib.ViewModels
{
    public class PartsViewModel : ViewModelBase, IRoutableViewModel
    {
        public PartsViewModel(IScreen hostScreen)
        {
            HostScreen = hostScreen ?? throw new ArgumentNullException(nameof(hostScreen));
            UrlPathSegment = Guid.NewGuid().ToString().Substring(0, 5);
        }

        /// <summary>
        /// Gets the <see cref="IScreen"/> that owns the routable view model.
        /// </summary>
        public IScreen HostScreen { get; }

        /// <summary>
        /// Gets the unique identifier for the routable view model.
        /// </summary>
        public string UrlPathSegment { get; }
    }
}