using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using KiCadDbLib.Services;
using KiCadDbLib.ViewModels;
using ReactiveUI;
using Splat;

namespace KiCadDbLib.Views
{
    public class PartsView : ReactiveUserControl<PartsViewModel>, IHosted
    {
        public PartsView()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                ViewModel!.SelectWorkspace.RegisterHandler(HandleSelectWorkspace)
                    .DisposeWith(disposables);

                var notifications = Locator.Current.GetRequiredService<INotificationPoster>();

                ViewModel.BuildLibrary.IsExecuting
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

                ViewModel.LoadParts.CanExecute
                    .Subscribe(
                    canExecute =>
                    {
                        if (canExecute)
                        {
                            ViewModel.LoadParts.Execute();
                        }
                    },
                    exception =>
                    {
                        notifications.ShowError("Load parts", exception);
                    })
                    .DisposeWith(disposables);
            });
        }

        /// <inheritdoc/>
        public Window? Host { get; set; }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private async Task HandleSelectWorkspace(InteractionContext<string?, string?> interactionContext)
        {
            if (Host is null)
            {
                return;
            }

            var currentWorkspace = interactionContext.Input;

            var dialog = new OpenFolderDialog()
            {
                Title = "Open Workspace",
                Directory = currentWorkspace,
            };

            var result = await dialog.ShowAsync(Host).ConfigureAwait(false);
            interactionContext.SetOutput(result ?? currentWorkspace);
        }
    }
}