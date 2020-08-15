using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using KiCadDbLib.Models;

namespace KiCadDbLib.Controls
{
    public class DataGridBehavior
    {

        public static readonly AttachedProperty<IEnumerable<ColumnInfo>> ColumnInfosProperty =
            AvaloniaProperty.RegisterAttached<DataGridBehavior, DataGrid, IEnumerable<ColumnInfo>>("ColumnInfos", Enumerable.Empty<ColumnInfo>());

        public static readonly AttachedProperty<ICommand> SelectionChangedCommandProperty =
                    AvaloniaProperty.RegisterAttached<DataGridBehavior, DataGrid, ICommand>("SelectionChangedCommand");
        static DataGridBehavior()
        {
            ColumnInfosProperty.Changed.Subscribe(OnColumnInfosChanged);
            SelectionChangedCommandProperty.Changed.Subscribe(OnSelectionChangedCommandChanged);
        }

        private static void OnSelectionChangedCommandChanged(AvaloniaPropertyChangedEventArgs obj)
        {
            if (!(obj.Sender is DataGrid target))
            {
                return;
            }

            target.SelectionChanged -= OnSelectionChanged;
            target.SelectionChanged += OnSelectionChanged;
        }

        public static IEnumerable<ColumnInfo> GetColumnInfos(DataGrid target)
        {
            return target.GetValue(ColumnInfosProperty);
        }

        public static ICommand GetSelectionChangedCommand(DataGrid target)
        {
            return target.GetValue(SelectionChangedCommandProperty);
        }


        public static void SetColumnInfos(DataGrid target, IEnumerable<ColumnInfo> value)
        {
            target.SetValue(ColumnInfosProperty, value);
        }

        public static void SetSelectionChangedCommand(DataGrid target, ICommand value)
        {
            target.SetValue(SelectionChangedCommandProperty, value);
        }

        private static void OnColumnInfosChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (!(e.Sender is DataGrid dg))
            {
                return;
            }

            if (!(e.NewValue is IEnumerable<ColumnInfo> columnInfos))
            {
                return;
            }

            dg.Columns.Clear();
            foreach (ColumnInfo columnInfo in columnInfos)
            {
                var column = new DataGridTextColumn()
                {
                    Header = columnInfo.Header,
                    Binding = new Binding(columnInfo.Path),
                };

                dg.Columns.Add(column);
            }
        }

        private static void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(sender is DataGrid target))
            {
                return;
            }

            if (target.SelectedItem is null)
            {
                return;
            }

            ICommand command = GetSelectionChangedCommand(target);

            if (command?.CanExecute(target.SelectedItem) == true)
            {
                command.Execute(target.SelectedItem);
            }
        }
    }
}
