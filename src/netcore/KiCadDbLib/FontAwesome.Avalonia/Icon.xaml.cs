using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
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

        public Icon()
        {
            AvaloniaXamlLoader.Load(this);
            IObservable<string> valueObservable = this.GetObservable(ValueProperty);
            IObservable<Drawing> drawingObservable = valueObservable.Select(ValueToDrawing);
            Bind(DrawingProperty, drawingObservable);
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

        private Drawing ValueToDrawing(string value)
        {
            string path = FontAwesomeIconProvider.GetIconPath(value);

            GeometryDrawing drawing = new GeometryDrawing()
            {
                Geometry = Geometry.Parse(path),
            };

            // Bind Foreground to icon foreground
            IObservable<IBrush> foregroundObservable = this.GetObservable(ForegroundProperty);
            drawing.Bind(GeometryDrawing.BrushProperty, foregroundObservable);

            return drawing;
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