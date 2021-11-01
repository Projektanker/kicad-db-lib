using ReactiveUI;

namespace KiCadDbLib.ViewModels
{
    public abstract class ViewModelBase : ReactiveObject, IActivatableViewModel
    {
        public ViewModelBase()
        {
            Activator = new ViewModelActivator();
        }

        /// <inheritdoc/>
        public ViewModelActivator Activator { get; }
    }
}