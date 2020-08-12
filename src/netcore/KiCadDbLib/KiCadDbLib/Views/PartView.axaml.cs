using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using KiCadDbLib.ViewModels;

namespace KiCadDbLib.Views
{
    public class PartView : ReactiveUserControl<PartViewModel>
    {
        public PartView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}