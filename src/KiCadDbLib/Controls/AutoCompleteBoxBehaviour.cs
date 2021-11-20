using System;
using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;

namespace KiCadDbLib.Controls
{
    public class AutoCompleteBoxBehaviour
    {
        public static readonly AttachedProperty<bool> FocusDropDownProperty =
                    AvaloniaProperty.RegisterAttached<AutoCompleteBoxBehaviour, AutoCompleteBox, bool>("FocusDropDown");

        static AutoCompleteBoxBehaviour()
        {
            FocusDropDownProperty.Changed.Subscribe(OnFocusDropDownChanged);
        }

        public static bool GetFocusDropDown(AutoCompleteBox target)
        {
            return target.GetValue(FocusDropDownProperty);
        }

        public static void SetFocusDropDown(AutoCompleteBox target, bool value)
        {
            target.SetValue(FocusDropDownProperty, value);
        }

        private static void CancelDropDownOpening(object? sender, CancelEventArgs e)
        {
            e.Cancel = true;
        }

        private static void OnDropDownOpened(object? sender, EventArgs e)
        {
            if (sender is not AutoCompleteBox acb)
            {
                return;
            }

            var dropdown = acb.FindControl<Popup>("PART_Popup");
            dropdown?.Focus();
        }

        private static void OnFocusDropDownChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Sender is not AutoCompleteBox acb)
            {
                return;
            }

            acb.DropDownOpened -= OnDropDownOpened;
            acb.DropDownOpened += OnDropDownOpened;
            acb.DropDownOpening += CancelDropDownOpening;
            acb.GotFocus -= OnGotFocus;
            acb.GotFocus += OnGotFocus;
            acb.LostFocus -= OnLostFocus;
            acb.LostFocus += OnLostFocus;
        }

        private static void OnGotFocus(object? sender, Avalonia.Input.GotFocusEventArgs e)
        {
            if (sender is not AutoCompleteBox acb)
            {
                return;
            }

            acb.DropDownOpening -= CancelDropDownOpening;
        }

        private static void OnLostFocus(object? sender, RoutedEventArgs e)
        {
            if (sender is not AutoCompleteBox acb)
            {
                return;
            }

            acb.DropDownOpening -= CancelDropDownOpening;
            acb.DropDownOpening += CancelDropDownOpening;
        }
    }
}