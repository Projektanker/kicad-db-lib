using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using ReactiveUI;

namespace KiCadDbLib.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IScreen
    {
        public MainWindowViewModel()
        {
            // Manage the routing state. Use the Router.Navigate.Execute
            // command to navigate to different view models. 
            //
            // Note, that the Navigate.Execute method accepts an instance 
            // of a view model, this allows you to pass parameters to 
            // your view models, or to reuse existing view models.
            //
            GoNext = ReactiveCommand.CreateFromObservable(
                () => Router.Navigate.Execute(new PartsViewModel(this))
            );

            Router = new RoutingState();            
        }

        public string Greeting => "Welcome to Avalonia!";

        // The Router associated with this Screen.
        // Required by the IScreen interface.
        public RoutingState Router { get; }

        // The command that navigates a user to first view model.
        public ReactiveCommand<Unit, IRoutableViewModel> GoNext { get; }

        // The command that navigates a user back.
        public ReactiveCommand<Unit, Unit> GoBack => Router.NavigateBack;
    }
}
