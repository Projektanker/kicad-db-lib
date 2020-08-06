using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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


        static DataGridBehavior()
        {
            ColumnInfosProperty.Changed.Subscribe(OnColumnInfosChanged);
        }

        public static IEnumerable<ColumnInfo> GetColumnInfos(DataGrid target)
        {
            return target.GetValue(ColumnInfosProperty);
        }

        public static void SetColumnInfos(DataGrid target, IEnumerable<ColumnInfo> value)
        {
            target.SetValue(ColumnInfosProperty, value);
        }

        private static void OnColumnInfosChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if(!(e.Sender is DataGrid dg))
            {
                return;
            }

            if(!(e.NewValue is IEnumerable<ColumnInfo> columnInfos))
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
    }
}
