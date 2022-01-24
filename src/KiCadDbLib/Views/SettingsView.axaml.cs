using System;
using System.Reactive.Disposables;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using KiCadDbLib.Services;
using KiCadDbLib.ViewModels;
using ReactiveUI;
using Splat;

namespace KiCadDbLib.Views
{
    public class SettingsView : ReactiveUserControl<SettingsViewModel>
    {
        public SettingsView()
        {
            this.WhenActivated(disposables =>
            {
                var notifications = Locator.Current.GetRequiredService<INotificationPoster>();
                ViewModel!.SaveSettings.ThrownExceptions
                    .Subscribe(exception => notifications.ShowError("Save", exception))
                    .DisposeWith(disposables);

                ViewModel.ImportCustomFields.ThrownExceptions
                    .Subscribe(exception => notifications.ShowError("Import", exception))
                    .DisposeWith(disposables);

                ViewModel.GoBack.ThrownExceptions
                    .Subscribe(exception => notifications.ShowError("Back", exception))
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