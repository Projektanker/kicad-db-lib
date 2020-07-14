using System;
using System.Collections.Generic;
using System.Text;
using Avalonia;
using Avalonia.Controls;

namespace FontAwesome.Avalonia
{
    /// <summary>
    /// Provides attached properties to set FontAwesome icons on controls.
    /// </summary>
    public class Awesome
    {
        static Awesome()
        {
            IconProperty.Changed.Subscribe(IconChanged);
        }


        /// <summary>
        /// Identifies the FontAwesome.Avalonia.Awesome.Content attached dependency property.
        /// </summary>  
        public static readonly AttachedProperty<string> IconProperty =
            AvaloniaProperty.RegisterAttached<Awesome, ContentControl, string>("Icon", string.Empty);

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
            if (!(evt.NewValue is string)) return;
            if (!(evt.Sender is ContentControl)) return;

            string iconKey = (string)evt.NewValue;
            var target = (evt.Sender as ContentControl); 
            var fa = new FontAwesome()
            {
                Icon = iconKey,
            };
            target.Content = fa;
        }
    }
}
