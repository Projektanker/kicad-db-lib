using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
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
                ViewModel.LoadParts
                    .Execute()
                    .Subscribe()
                    .DisposeWith(disposables);

                var notifications = Locator.Current.GetRequiredService<INotificationPoster>();
                if (notifications != null)
                {
                    ViewModel.BuildLibrary.IsExecuting
                        .Where(isExecuting => isExecuting)
                        .Do(_ => notifications.ShowInformation("Build", "Start of build."))
                        .Subscribe()
                        .DisposeWith(disposables);

                    ViewModel.BuildLibrary
                        .Do(_ => notifications.ShowSuccess("Build", "Build successful."))
                        .Subscribe()
                        .DisposeWith(disposables);

                    ViewModel.BuildLibrary
                        .ThrownExceptions
                        .Do(exception => notifications.ShowError("Build", exception.Message))
                        .Subscribe()
                        .DisposeWith(disposables);
                }
            });

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}