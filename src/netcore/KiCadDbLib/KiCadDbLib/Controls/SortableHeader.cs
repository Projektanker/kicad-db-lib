
using System;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using DynamicData.Binding;
using Projektanker.Icons.Avalonia;

namespace KiCadDbLib.Controls
{
    public class SortableHeader : TemplatedControl
    {
        public static readonly StyledProperty<string> HeaderProperty =
           AvaloniaProperty.Register<SortableHeader, string>(nameof(Header));

        public static readonly StyledProperty<SortDirection?> SortDirectionProperty =
            AvaloniaProperty.Register<SortableHeader, SortDirection?>(nameof(SortDirection));

        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            base.OnPointerReleased(e);
            SortDirection = SortDirection switch
            {
                DynamicData.Binding.SortDirection.Ascending => DynamicData.Binding.SortDirection.Descending,
                DynamicData.Binding.SortDirection.Descending => null,
                _ => DynamicData.Binding.SortDirection.Ascending,
            };
        }


        public string Header
        {
            get => GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        public SortDirection? SortDirection
        {
            get => GetValue(SortDirectionProperty);
            set => SetValue(SortDirectionProperty, value);
        }

    }
}