using System;
using System.Collections.Generic;
using System.Data.Common;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Utils;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using KiCadDbLib.ViewModels;
using ReactiveUI;

namespace KiCadDbLib.Views
{
    public class PartsView : ReactiveUserControl<PartsViewModel>
    {
        public PartsView()
        {
            this.WhenActivated(disposables => { });
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            //InitializePartsGrid();
        }

        private void InitializePartsGrid()
        {
            DataGrid partsGrid = this.FindControl<DataGrid>("partsGrid");
            partsGrid.AutoGeneratingColumn += PartsGridAutoGeneratingColumn;
        }

        private void PartsGridAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (!(sender is DataGrid _))
            {
                return;
            }

            

            e.Column.Header = e.Column.Header.ToString().ToUpperInvariant();
            return;

            if (e.PropertyType.IsGenericType &&
                typeof(Dictionary<,>) == e.PropertyType.GetGenericTypeDefinition())
            {
                (e.Column as DataGridBoundColumn).Binding = new Binding("CustomFields[MF]");
                return;
            }

            e.Cancel = Type.GetTypeCode(e.PropertyType) switch
            {
                TypeCode.Object => true,
                TypeCode.Empty => true,
                TypeCode.DBNull => true,
                _ => false,
            };
        }
    }
}
