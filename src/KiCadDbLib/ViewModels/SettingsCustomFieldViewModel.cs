using System;
using System.Reactive;
using ReactiveUI;

namespace KiCadDbLib.ViewModels
{
    public sealed class SettingsCustomFieldViewModel : ViewModelBase
    {
        private readonly Action<string> _removeAction;

        public SettingsCustomFieldViewModel(string value, Action<string> removeAction)
        {
            Value = value;
            _removeAction = removeAction ?? throw new ArgumentNullException(nameof(removeAction));
            Remove = ReactiveCommand.Create(execute: ExecuteRemove);
        }

        public ReactiveCommand<Unit, Unit> Remove { get; }

        public string Value
        {
            get;
        }

        private void ExecuteRemove()
        {
            _removeAction(Value);
        }
    }
}