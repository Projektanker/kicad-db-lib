using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using KiCadDbLib.Services;
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
                var notifications = Locator.Current.GetRequiredService<INotificationPoster>();

                ViewModel!.BuildLibrary.IsExecuting
                    .Where(isExecuting => isExecuting)
                    .Subscribe(_ => notifications.ShowInformation("Build", "Running"))
                    .DisposeWith(disposables);

                ViewModel.BuildLibrary
                    .Subscribe(_ => notifications.ShowSuccess("Build", "Done"))
                    .DisposeWith(disposables);

                ViewModel.BuildLibrary
                    .ThrownExceptions
                    .Subscribe(exception => notifications.ShowError("Build", exception))
                    .DisposeWith(disposables);

                ViewModel.LoadParts.ThrownExceptions
                    .Subscribe(exception => notifications.ShowError("Load parts", exception))
                    .DisposeWith(disposables);

                ViewModel.LoadParts
                    .Execute()
                    .Subscribe(_ => { }, exception => notifications.ShowError("Load parts", exception))
                    .DisposeWith(disposables);
            });

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}