using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.ReactiveUI;
using KiCadDbLib.ViewModels;
using ReactiveUI;

namespace KiCadDbLib.Views
{
    public class PartView : ReactiveUserControl<PartsViewModel>
    {
        public PartView()
        {
            this.WhenActivated(disposables => { });
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
