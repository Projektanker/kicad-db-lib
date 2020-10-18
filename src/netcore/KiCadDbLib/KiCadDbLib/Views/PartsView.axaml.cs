using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using KiCadDbLib.ViewModels;
using ReactiveUI;
using Splat;

namespace KiCadDbLib.Views
{
    public class PartsView : ReactiveUserControl<PartsViewModel>
    {
        public PartsView()
        {
            this.WhenActivated(disposables =>
            {
                ViewModel.LoadParts
                    .Execute()
                    .Subscribe()
                    .DisposeWith(disposables);

                INotificationManager notificationManager = Locator.Current.GetService<INotificationManager>();
                if (notificationManager != null)
                {
                    ViewModel.BuildLibrary.IsExecuting
                        .Where(isExecuting => isExecuting)
                        .Do(_ => notificationManager.ShowInformation("Build", "Start of build."))
                        .Subscribe()
                        .DisposeWith(disposables);

                    ViewModel.BuildLibrary
                        .Do(_ => notificationManager.ShowSuccess("Build", "Build successful."))
                        .Subscribe()
                        .DisposeWith(disposables);

                    ViewModel.BuildLibrary
                        .ThrownExceptions
                        .Do(exception => notificationManager.ShowError("Build", exception.Message))
                        .Subscribe()
                        .DisposeWith(disposables);
                }
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