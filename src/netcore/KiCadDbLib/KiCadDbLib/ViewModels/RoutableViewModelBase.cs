using System;
using System.Reactive.Disposables;
using ReactiveUI;

namespace KiCadDbLib.ViewModels
{
    public abstract class RoutableViewModelBase : ViewModelBase, IRoutableViewModel
    {
        protected RoutableViewModelBase(IScreen hostScreen)
        {
            HostScreen = hostScreen ?? throw new ArgumentNullException(nameof(hostScreen));
            UrlPathSegment = GetType().Name
                .Replace("ViewModel", string.Empty, StringComparison.OrdinalIgnoreCase);

            this.WhenActivated(WhenActivated);
        }

        /// <inheritdoc/>
        public IScreen HostScreen { get; }

        /// <inheritdoc/>
        public string UrlPathSegment { get; }

        protected virtual void WhenActivated(CompositeDisposable disposables)
        {
        }
    }
}