using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.LogicalTree;

namespace FontAwesome.Avalonia
{
    /// <summary>
    /// Provides attached properties to set FontAwesome icons on controls.
    /// </summary>
    public class Attached
    {
        static Attached()
        {
            IconProperty.Changed.Subscribe(IconChanged);
        }

        /// <summary>
        /// Identifies the FontAwesome.Avalonia.Awesome.Content attached dependency property.
        /// </summary>
        public static readonly AttachedProperty<string> IconProperty =
            AvaloniaProperty.RegisterAttached<Attached, ContentControl, string>("Icon", string.Empty);

        public static string GetIcon(ContentControl target)
        {
            return target.GetValue(IconProperty);
        }

        public static void SetIcon(ContentControl target, string value)
        {
            target.SetValue(IconProperty, value);
        }

        private static void IconChanged(AvaloniaPropertyChangedEventArgs evt)
        {
            if (!(evt.NewValue is string value))
            {
                return;
            }
            if (!(evt.Sender is ContentControl target))
            {
                return;
            }

            var fa = new Icon()
            {
                Value = value,
            };

            target.Content = fa;
        }
    }
}