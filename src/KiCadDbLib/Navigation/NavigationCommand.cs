using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI;

namespace KiCadDbLib.Navigation
{
    public static class NavigationCommand
    {
        public static ReactiveCommand<Unit, IRoutableViewModel> Create<TViewModel>(IScreen hostScreen, Func<TViewModel> viewModelFactory, IObservable<bool>? canExecute = null)
            where TViewModel : IRoutableViewModel
        {
            return ReactiveCommand.CreateFromObservable(
                execute: () => hostScreen.Router.Navigate.Execute(viewModelFactory.Invoke()),
                canExecute: canExecute);
        }

        public static ReactiveCommand<TParam, IRoutableViewModel> Create<TParam, TViewModel>(IScreen hostScreen, Func<TParam, Task<TViewModel>> viewModelFactory, IObservable<bool>? canExecute = null)
            where TViewModel : IRoutableViewModel
        {
            return ReactiveCommand.CreateFromTask(
                execute: (TParam param) => NavigateAsync(hostScreen, viewModelFactory, param),
                canExecute: canExecute);
        }

        private static async Task<IRoutableViewModel> NavigateAsync<TParam, TViewModel>(IScreen hostScreen, Func<TParam, Task<TViewModel>> viewModelFactory, TParam param)
            where TViewModel : IRoutableViewModel
        {
            TViewModel vm = await viewModelFactory(param).ConfigureAwait(true);
            return await hostScreen.Router.Navigate.Execute(vm);
        }
    }
}