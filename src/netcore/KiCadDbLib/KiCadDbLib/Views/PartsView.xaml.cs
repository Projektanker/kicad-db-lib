using System;
using System.Collections.Generic;
using System.Data.Common;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Utils;
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
            InitializePartsGrid();
        }

        private void InitializePartsGrid()
        {
            DataGrid partsGrid = this.FindControl<DataGrid>("partsGrid");
            partsGrid.AutoGeneratingColumn += PartsGridAutoGeneratingColumn;
        }

        private void PartsGridAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if(!(sender is DataGrid _))
            {
                return;
            }

            e.Column.Header = e.Column.Header.ToString().ToUpperInvariant();

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
