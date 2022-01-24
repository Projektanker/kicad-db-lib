using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using KiCadDbLib.Services;
using KiCadDbLib.ViewModels;
using Material.Dialog;
using Material.Dialog.Icons;
using ReactiveUI;
using Splat;

namespace KiCadDbLib.Views
{
    public class PartView : ReactiveUserControl<PartViewModel>, IHosted
    {
        public PartView()
        {
            InitializeComponent();
            this.WhenActivated(disposables =>
            {
                ViewModel!.DeletePartConfirmation
                    .RegisterHandler(HandleDeletePartConfirmationAsync)
                    .DisposeWith(disposables);

                ViewModel.DiscardChangesConfirmation
                    .RegisterHandler(HandleDiscardChangesConfirmationAsync)
                    .DisposeWith(disposables);

                var notifications = Locator.Current.GetRequiredService<INotificationPoster>();
                ViewModel.Save
                    .Subscribe(_ => notifications.ShowSuccess("Save + Build", "Done"))
                    .DisposeWith(disposables);

                ViewModel.Save.ThrownExceptions
                    .Subscribe(exception => notifications.ShowError("Save + Build", exception))
                    .DisposeWith(disposables);

                ViewModel.Save.IsExecuting
                    .Where(isExecuting => isExecuting)
                    .Subscribe(_ => notifications.ShowInformation("Save + Build", "Running."))
                    .DisposeWith(disposables);
            });
        }

        /// <inheritdoc/>
        public Window Host { get; set; }

        private async Task HandleDeletePartConfirmationAsync(InteractionContext<Unit, bool> interactionContext)
        {
            var dialog = DialogHelper.CreateAlertDialog(new AlertDialogBuilderParams()
            {
                ContentHeader = "Confirm action",
                SupportingText = "Are you sure to delete the part?",
                DialogHeaderIcon = DialogIconKind.Help,
                StartupLocation = WindowStartupLocation.CenterOwner,
                NegativeResult = new DialogResult("no"),
                Borderless = true,
                DialogButtons = new[]
                {
                    new DialogResultButton
                    {
                        Content = "Yes",
                        Result = "yes",
                    },
                    new DialogResultButton
                    {
                        Content = "No",
                        Result = "no",
                    },
                },
            });

            var result = await dialog.ShowDialog(Host)
                .ConfigureAwait(false);
            interactionContext.SetOutput(result.GetResult == "yes");
        }

        private async Task HandleDiscardChangesConfirmationAsync(InteractionContext<Unit, bool> interactionContext)
        {
            var dialog = DialogHelper.CreateAlertDialog(new AlertDialogBuilderParams()
            {
                ContentHeader = "Confirm action",
                SupportingText = "Are you sure to discard the changes?",
                DialogHeaderIcon = DialogIconKind.Help,
                StartupLocation = WindowStartupLocation.CenterOwner,
                NegativeResult = new DialogResult("no"),
                Borderless = true,
                DialogButtons = new[]
                {
                    new DialogResultButton
                    {
                        Content = "Yes",
                        Result = "yes",
                    },
                    new DialogResultButton
                    {
                        Content = "No",
                        Result = "no",
                    },
                },
            });

            var result = await dialog.ShowDialog(Host)
                .ConfigureAwait(false);
            interactionContext.SetOutput(result.GetResult == "yes");
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}