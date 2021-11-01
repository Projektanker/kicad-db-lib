using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using KiCadDbLib.ViewModels;

namespace KiCadDbLib.Views
{
    public class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}