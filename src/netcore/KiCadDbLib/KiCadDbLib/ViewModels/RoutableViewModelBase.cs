using System;
using System.Reactive.Disposables;
using ReactiveUI;

namespace KiCadDbLib.ViewModels
{
    public abstract class RoutableViewModelBase : ViewModelBase, IRoutableViewModel
    {
        public RoutableViewModelBase(IScreen hostScreen)
        {
            HostScreen = hostScreen ?? throw new ArgumentNullException(nameof(hostScreen));
            UrlPathSegment = GetType().Name
                .Replace("ViewModel", "", StringComparison.OrdinalIgnoreCase)
                .ToLowerInvariant();

            this.WhenActivated(WhenActivated);
        }

        public IScreen HostScreen { get; }
        public string UrlPathSegment { get; }

        protected virtual void WhenActivated(CompositeDisposable disposables)
        {

        }
    }
}