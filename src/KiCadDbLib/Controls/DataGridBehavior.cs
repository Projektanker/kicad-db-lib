﻿using System;
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
        public static readonly AttachedProperty<ICommand> CellPointerPressedCommandProperty =
            AvaloniaProperty.RegisterAttached<DataGridBehavior, DataGrid, ICommand>("CellPointerPressedCommand");

        public static readonly AttachedProperty<IEnumerable<ColumnInfo>> ColumnInfosProperty =
            AvaloniaProperty.RegisterAttached<DataGridBehavior, DataGrid, IEnumerable<ColumnInfo>>("ColumnInfos", Enumerable.Empty<ColumnInfo>());

        public static readonly AttachedProperty<ICommand> SelectionChangedCommandProperty =
            AvaloniaProperty.RegisterAttached<DataGridBehavior, DataGrid, ICommand>("SelectionChangedCommand");

        static DataGridBehavior()
        {
            ColumnInfosProperty.Changed.Subscribe(OnColumnInfosChanged);
            SelectionChangedCommandProperty.Changed.Subscribe(OnSelectionChangedCommandChanged);
            CellPointerPressedCommandProperty.Changed.Subscribe(OnCellPointerPressedCommandChanged);
        }

        public static ICommand GetCellPointerPressedCommand(DataGrid target)
        {
            if (target is null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            return target.GetValue(CellPointerPressedCommandProperty);
        }

        public static IEnumerable<ColumnInfo> GetColumnInfos(DataGrid target)
        {
            if (target is null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            return target.GetValue(ColumnInfosProperty);
        }

        public static ICommand GetSelectionChangedCommand(DataGrid target)
        {
            if (target is null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            return target.GetValue(SelectionChangedCommandProperty);
        }

        public static void SetCellPointerPressedCommand(DataGrid target, ICommand value)
        {
            if (target is null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            target.SetValue(CellPointerPressedCommandProperty, value);
        }

        public static void SetColumnInfos(DataGrid target, IEnumerable<ColumnInfo> value)
        {
            if (target is null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            target.SetValue(ColumnInfosProperty, value);
        }

        public static void SetSelectionChangedCommand(DataGrid target, ICommand value)
        {
            if (target is null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            target.SetValue(SelectionChangedCommandProperty, value);
        }

        private static void OnCellPointerPressed(object? sender, DataGridCellPointerPressedEventArgs e)
        {
            if (sender is not DataGrid target)
            {
                return;
            }

            if (e.Row is null)
            {
                return;
            }

            ICommand command = GetCellPointerPressedCommand(target);

            if (command?.CanExecute(e.Row.DataContext) ?? false)
            {
                command.Execute(e.Row.DataContext);
            }
        }

        private static void OnCellPointerPressedCommandChanged(AvaloniaPropertyChangedEventArgs obj)
        {
            if (obj.Sender is not DataGrid target)
            {
                return;
            }

            target.CellPointerPressed -= OnCellPointerPressed;
            target.CellPointerPressed += OnCellPointerPressed;
        }

        private static void OnColumnInfosChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Sender is not DataGrid dg)
            {
                return;
            }

            if (e.NewValue is not IEnumerable<ColumnInfo> columnInfos)
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

        private static void OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (sender is not DataGrid target)
            {
                return;
            }

            if (e.AddedItems.Count == 0 || target.SelectedItem is null)
            {
                return;
            }

            ICommand command = GetSelectionChangedCommand(target);

            if (command?.CanExecute(target.SelectedItem) ?? false)
            {
                command.Execute(target.SelectedItem);
            }
        }

        private static void OnSelectionChangedCommandChanged(AvaloniaPropertyChangedEventArgs obj)
        {
            if (obj.Sender is not DataGrid target)
            {
                return;
            }

            target.SelectionChanged -= OnSelectionChanged;
            target.SelectionChanged += OnSelectionChanged;
        }
    }
}