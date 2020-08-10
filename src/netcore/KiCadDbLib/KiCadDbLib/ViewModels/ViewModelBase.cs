using ReactiveUI;

namespace KiCadDbLib.ViewModels
{
    public abstract class ViewModelBase : ReactiveObject, IActivatableViewModel
    {
        public ViewModelBase()
        {
            Activator = new ViewModelActivator();
        }

        public ViewModelActivator Activator { get; }
    }
}