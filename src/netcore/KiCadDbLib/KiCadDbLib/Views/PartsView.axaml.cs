using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reactive.Disposables;
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
            this.WhenActivated(disposables => {
                var vm = DataContext as PartsViewModel;
                vm.LoadParts
                    .Execute()
                    .Subscribe()
                    .DisposeWith(disposables);
            });

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
            partsGrid.SelectionChanged += PartsGridSelectionChanged;
        }

        private void PartsGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            return;
        }
    }
}
