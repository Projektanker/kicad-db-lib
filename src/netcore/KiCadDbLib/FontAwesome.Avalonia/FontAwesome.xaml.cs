using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Avalonia.Metadata;

namespace FontAwesome.Avalonia
{
    public class FontAwesome : UserControl
    {
        public static readonly DirectProperty<FontAwesome, Drawing> DrawingProperty =
        AvaloniaProperty.RegisterDirect<FontAwesome, Drawing>(
            nameof(Drawing),
            o => o.Drawing);

        public static readonly StyledProperty<string> IconProperty =
                    AvaloniaProperty.Register<FontAwesome, string>(nameof(Icon));

        private Drawing _drawing;

        static FontAwesome()
        {
            IconProperty.Changed.Subscribe(OnIconPropertyChanged);
        }

        public Drawing Drawing
        {
            get => _drawing;
            private set => SetAndRaise(DrawingProperty, ref _drawing, value);
        }

        public string Icon
        {
            get => GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        private static void OnIconPropertyChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (!(e.Sender is FontAwesome target))
            {
                return;
            }

            string path = IconHelper.GetIconPath(target.Icon);

            target.Drawing = new GeometryDrawing()
            {
                Geometry = Geometry.Parse(path),
                Brush = target.Foreground,
            };
        }
    }
}