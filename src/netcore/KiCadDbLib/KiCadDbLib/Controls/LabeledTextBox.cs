
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using Avalonia.Metadata;

namespace KiCadDbLib.Controls
{
    public class LabeledTextBox : TemplatedControl
    {

        public static readonly StyledProperty<bool> IsRequiredProperty =
            AvaloniaProperty.Register<LabeledTextBox, bool>(nameof(IsRequired));

        public static readonly StyledProperty<string> IsRequiredAsteriskProperty =
           AvaloniaProperty.Register<LabeledTextBox, string>("IsRequiredAsterisk");

        public static readonly StyledProperty<string> LabelProperty =
            AvaloniaProperty.Register<LabeledTextBox, string>(nameof(Label));

        public static readonly DirectProperty<LabeledTextBox, string> TextProperty =
            AvaloniaProperty.RegisterDirect<LabeledTextBox, string>(
                nameof(Text),
                labeledTextBox => labeledTextBox.Text,
                (labeledTextBox, value) => labeledTextBox.Text = value,
                defaultBindingMode: BindingMode.TwoWay,
                enableDataValidation: true);

        private string _text;

        public LabeledTextBox()
        {
        }

        public bool IsRequired
        {
            get => GetValue(IsRequiredProperty);
            set
            {
                SetValue(IsRequiredProperty, value);
                SetValue(IsRequiredAsteriskProperty, value ? "*" : string.Empty);
            }
        }

        public string Label
        {
            get => GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }

        [Content]
        public string Text
        {
            get => _text;
            set => SetAndRaise(TextProperty, ref _text, value);
        }
    }
}