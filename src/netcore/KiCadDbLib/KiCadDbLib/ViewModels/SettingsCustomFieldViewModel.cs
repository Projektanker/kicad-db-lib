using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;

namespace KiCadDbLib.ViewModels
{
    public sealed class SettingsCustomFieldViewModel : ViewModelBase, IEquatable<SettingsCustomFieldViewModel>
    {
        private readonly ICollection<SettingsCustomFieldViewModel> _parent;
        private string _value;

        public SettingsCustomFieldViewModel(ICollection<SettingsCustomFieldViewModel> parent, string value)
        {
            _parent = parent ?? throw new ArgumentNullException(nameof(parent));
            _value = value;
            IObservable<bool> canAdd = this.WhenAnyValue(
                vm => vm.Value,
                changedValue =>
                {
                    return !string.IsNullOrWhiteSpace(changedValue)
                        && !_parent.Contains(this);
                });

            Add = ReactiveCommand.Create(execute: ExectuteAdd, canExecute: canAdd);
            Remove = ReactiveCommand.Create(execute: ExectuteRemove);
        }

        public ReactiveCommand<Unit, Unit> Add { get; }

        public ReactiveCommand<Unit, Unit> Remove { get; }

        public string Value
        {
            get => _value;
            set => this.RaiseAndSetIfChanged(ref _value, value);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SettingsCustomFieldViewModel);
        }

        public bool Equals(SettingsCustomFieldViewModel other)
        {
            return other != null &&
                   _value == other._value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_value);
        }

        private void ExectuteAdd()
        {
            _parent.Add(new SettingsCustomFieldViewModel(_parent, Value));
            Value = string.Empty;
        }

        private void ExectuteRemove()
        {
            _parent.Remove(this);
        }
    }
}