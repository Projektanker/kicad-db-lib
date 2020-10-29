using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.ReactiveUI;
using KiCadDbLib.ViewModels;
using MessageBox.Avalonia;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using ReactiveUI;
using Splat;

namespace KiCadDbLib.Views
{
    public class PartView : ReactiveUserControl<PartViewModel>, IHosted
    {
        public PartView()
        {
            this.InitializeComponent();
            this.WhenActivated(disposables =>
            {
                ViewModel.DeletePartConfirmation
                .RegisterHandler(HandleDeletePartConfirmationAsync)
                .DisposeWith(disposables);

                ViewModel.DiscardChangesConfirmation
                .RegisterHandler(HandleDiscardChangesConfirmationAsync)
                .DisposeWith(disposables);

                INotificationManager notificationManager = Locator.Current.GetService<INotificationManager>();
                if (notificationManager != null)
                {
                    ViewModel.Save
                        .Do(_ => notificationManager.ShowSuccess("Save and Build", "Save and build successful."))
                        .Subscribe()
                        .DisposeWith(disposables);

                    ViewModel.Save.ThrownExceptions
                        .Do(exception => notificationManager.ShowError("Save and Build", exception.Message))
                        .Subscribe()
                        .DisposeWith(disposables);

                    ViewModel.Save.IsExecuting
                        .Where(isExecuting => isExecuting)
                        .Do(_ => notificationManager.ShowInformation("Save and Build", "Start of save and build."))
                        .Subscribe()
                        .DisposeWith(disposables);
                }
            });
        }

        public Window Host { get; set; }

        private async Task HandleDeletePartConfirmationAsync(InteractionContext<Unit, bool> interactionContext)
        {
            var msb = MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams()
                {
                    ButtonDefinitions = ButtonEnum.YesNo,
                    ContentMessage = "Delete?",
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    CanResize = false,
                    // WindowIcon = Host.Icon,
                });

            var buttonResult = await msb.ShowDialog(Host);
            interactionContext.SetOutput(buttonResult == ButtonResult.Yes);
        }

        private async Task HandleDiscardChangesConfirmationAsync(InteractionContext<Unit, bool> interactionContext)
        {
            var msb = MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams()
                {
                    ButtonDefinitions = ButtonEnum.YesNo,
                    ContentMessage = "Discard changes?",
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    CanResize = false,
                    // WindowIcon = _iconBitmap,
                });

            var buttonResult = await msb.ShowDialog(Host);
            interactionContext.SetOutput(buttonResult == ButtonResult.Yes);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}