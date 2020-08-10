using Avalonia.Controls.Templates;
using ReactiveUI;

namespace KiCadDbLib
{
    public class RoutingViewLocator : IViewLocator
    {
        private readonly IDataTemplate _viewLocator = new ViewLocator();

        public IViewFor ResolveView<T>(T viewModel, string contract = null) 
            where T : class
        {
            return _viewLocator.Build(viewModel) as IViewFor;
        }
    }
}