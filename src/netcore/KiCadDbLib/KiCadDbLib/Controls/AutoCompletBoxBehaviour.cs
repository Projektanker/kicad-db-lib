using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Interactivity;
using KiCadDbLib.Models;

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

        
        private static void OnFocusDropDownChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (!(e.Sender is AutoCompleteBox acb))
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

        private static void CancelDropDownOpening(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
        }

        private static void OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (!(sender is AutoCompleteBox acb))
            {
                return;
            }

            acb.DropDownOpening -= CancelDropDownOpening;
            acb.DropDownOpening += CancelDropDownOpening;
        }

        private static void OnGotFocus(object sender, Avalonia.Input.GotFocusEventArgs e)
        {
            if (!(sender is AutoCompleteBox acb))
            {
                return;
            }

            acb.DropDownOpening -= CancelDropDownOpening;
        }

        private static void OnDropDownOpened(object sender, EventArgs e)
        {
            if (!(sender is AutoCompleteBox acb))
            {
                return;
            }
            Debug.WriteLine("PopupFocus");

            var dropdown = acb.FindControl<Popup>("PART_Popup");
            dropdown?.Focus();
        }

    }
}
