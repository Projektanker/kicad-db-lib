using ReactiveUI;

namespace KiCadDbLib.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IScreen
    {
        public MainWindowViewModel()
        {
            Router = new RoutingState();
            Router.NavigateAndReset.Execute(new PartsViewModel(this));
        }

        /// <inheritdoc/>
        public RoutingState Router { get; }
    }
}