using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace KiCadDbLib.Controls
{
    public class FormControl : UserControl
    {
        public FormControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}