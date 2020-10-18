using System;
using System.Reactive;
using Avalonia.Controls.Notifications;
using KiCadDbLib.Services;
using ReactiveUI;
using Splat;

namespace KiCadDbLib.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IScreen
    {
        public MainWindowViewModel()
        {
            // Manage the routing state. Use the Router.Navigate.Execute command to navigate to
            // different view models.
            //
            // Note, that the Navigate.Execute method accepts an instance of a view model, this
            // allows you to pass parameters to your view models, or to reuse existing view models.
            GoNext = ReactiveCommand.CreateFromObservable(() =>
                {
                    var settingsService = new SettingsService();
                    var partsService = new PartsService(settingsService);
                    return Router.Navigate.Execute(new SettingsViewModel(this));
                });

            Router = new RoutingState();
            Router.NavigateAndReset.Execute(new PartsViewModel(this));
        }

        public string Greeting => "Welcome to Avalonia!";

        // The Router associated with this Screen. Required by the IScreen interface.
        public RoutingState Router { get; }

        // The command that navigates a user to first view model.
        public ReactiveCommand<Unit, IRoutableViewModel> GoNext { get; }

        // The command that navigates a user back.
        public ReactiveCommand<Unit, Unit> GoBack => Router.NavigateBack;
    }
}