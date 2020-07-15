using System;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Media;
using Avalonia.Metadata;
using FontAwesome.Avalonia.FontAwesome;

namespace FontAwesome.Avalonia
{
    public class Icon : TemplatedControl
    {
        public static readonly DirectProperty<Icon, Drawing> DrawingProperty =
        AvaloniaProperty.RegisterDirect<Icon, Drawing>(
            nameof(Drawing),
            o => o.Drawing);

        public static readonly StyledProperty<string> ValueProperty =
                    AvaloniaProperty.Register<Icon, string>(nameof(Value));

        private Drawing _drawing;

        static Icon()
        {
            ValueProperty.Changed.Subscribe(OnIconPropertyChanged);
            // ForegroundProperty.Changed.Subscribe(OnForegroundPropertyChanged);
        }

        private static void OnForegroundPropertyChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (!(e.Sender is Icon target))
            {
                return;
            }

            switch (target.Drawing)
            {
                case GeometryDrawing geometryDrawing:
                    geometryDrawing.Brush = target.Foreground;
                    break;

                default:
                    break;
            }
        }

        public Drawing Drawing
        {
            get => _drawing;
            private set => SetAndRaise(DrawingProperty, ref _drawing, value);
        }

        public string Value
        {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        private static void OnIconPropertyChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (!(e.Sender is Icon icon))
            {
                return;
            }

            string path = FontAwesomeIconProvider.GetIconPath(icon.Value);

            GeometryDrawing drawing = new GeometryDrawing()
            {
                Geometry = Geometry.Parse(path),
            };

            // Bind Foreground to icon foreground
            IObservable<IBrush> foregroundObservable = icon.GetObservable(ForegroundProperty);
            drawing.Bind(GeometryDrawing.BrushProperty, foregroundObservable);

            icon.Drawing = drawing;
        }
    }
}