using System;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;

namespace KiCadDbLib.Navigation
{
    public static class NavigationCommand
    {
        public static ReactiveCommand<Unit, IRoutableViewModel> Create<TViewModel>(IScreen hostScreen, Func<TViewModel> viewModelFactory)
            where TViewModel : IRoutableViewModel
        {
            return ReactiveCommand.CreateFromObservable(
                execute: () => hostScreen.Router.Navigate.Execute(viewModelFactory.Invoke()),
                canExecute: hostScreen.Router.CurrentViewModel.Select(routableViewModel => !(routableViewModel is TViewModel)));
        }
    }
}