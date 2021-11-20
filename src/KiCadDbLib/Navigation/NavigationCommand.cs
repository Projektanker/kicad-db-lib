using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI;

namespace KiCadDbLib.Navigation
{
    public static class NavigationCommand
    {
        public static ReactiveCommand<Unit, IRoutableViewModel> Create<TViewModel>(IScreen hostScreen, Func<TViewModel> viewModelFactory)
            where TViewModel : IRoutableViewModel
        {
            if (hostScreen is null)
            {
                throw new ArgumentNullException(nameof(hostScreen));
            }

            if (viewModelFactory is null)
            {
                throw new ArgumentNullException(nameof(viewModelFactory));
            }

            return ReactiveCommand.CreateFromObservable(
                execute: () => hostScreen.Router.Navigate.Execute(viewModelFactory.Invoke()),
                canExecute: GetCanExecute<TViewModel>(hostScreen));
        }

        public static ReactiveCommand<TParam, IRoutableViewModel> Create<TParam, TViewModel>(IScreen hostScreen, Func<TParam, TViewModel> viewModelFactory)
            where TViewModel : IRoutableViewModel
        {
            if (hostScreen is null)
            {
                throw new ArgumentNullException(nameof(hostScreen));
            }

            if (viewModelFactory is null)
            {
                throw new ArgumentNullException(nameof(viewModelFactory));
            }

            return ReactiveCommand.CreateFromObservable(
                execute: (TParam param) => hostScreen.Router.Navigate.Execute(viewModelFactory.Invoke(param)),
                canExecute: GetCanExecute<TViewModel>(hostScreen));
        }

        public static ReactiveCommand<TParam, IRoutableViewModel> Create<TParam, TViewModel>(IScreen hostScreen, Func<TParam, Task<TViewModel>> viewModelFactory)
            where TViewModel : IRoutableViewModel
        {
            if (hostScreen is null)
            {
                throw new ArgumentNullException(nameof(hostScreen));
            }

            if (viewModelFactory is null)
            {
                throw new ArgumentNullException(nameof(viewModelFactory));
            }

            return ReactiveCommand.CreateFromTask(
                execute: (TParam param) => NavigateAsync(hostScreen, viewModelFactory, param),
                canExecute: GetCanExecute<TViewModel>(hostScreen));
        }

        private static async Task<IRoutableViewModel> NavigateAsync<TParam, TViewModel>(IScreen hostScreen, Func<TParam, Task<TViewModel>> viewModelFactory, TParam param)
            where TViewModel : IRoutableViewModel
        {
            TViewModel vm = await viewModelFactory(param).ConfigureAwait(false);
            return await hostScreen.Router.Navigate.Execute(vm);
        }

        private static IObservable<bool> GetCanExecute<TViewModel>(IScreen hostScreen)
        {
            return hostScreen.Router.CurrentViewModel.Select(routableViewModel => !(routableViewModel is TViewModel));
        }
    }
}