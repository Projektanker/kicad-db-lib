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
                canExecute: GetCanExecute<TViewModel>(hostScreen));
        }

        public static ReactiveCommand<TParam, IRoutableViewModel> Create<TParam, TViewModel>(IScreen hostScreen, Func<TParam, TViewModel> viewModelFactory)
            where TViewModel : IRoutableViewModel
        {
            return ReactiveCommand.CreateFromObservable(
                execute: (TParam param) => hostScreen.Router.Navigate.Execute(viewModelFactory.Invoke(param)),
                canExecute: GetCanExecute<TViewModel>(hostScreen));
        }

        private static IObservable<bool> GetCanExecute<TViewModel>(IScreen hostScreen)
        {
            return hostScreen.Router.CurrentViewModel.Select(routableViewModel => !(routableViewModel is TViewModel));
        }
    }
}