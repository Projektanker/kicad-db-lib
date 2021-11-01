using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using KiCadDbLib.ViewModels;
using ReactiveUI;

namespace KiCadDbLib.Views
{
    public class AboutView : ReactiveUserControl<AboutViewModel>
    {
        public AboutView()
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