using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using KiCadDbLib.ViewModels;
using ReactiveUI;

namespace KiCadDbLib.Views
{
    public class PartView : ReactiveUserControl<PartViewModel>
    {
        public PartView()
        {
            this.WhenActivated(disposables => { });
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}