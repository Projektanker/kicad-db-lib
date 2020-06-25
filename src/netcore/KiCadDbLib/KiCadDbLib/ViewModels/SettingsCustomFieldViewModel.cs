using System;
using System.Collections.Generic;
using System.Linq;
using ReactiveUI;

namespace KiCadDbLib.ViewModels
{
    public sealed class SettingsCustomFieldViewModel : ViewModelBase, IEquatable<SettingsCustomFieldViewModel>
    {
        private readonly ICollection<SettingsCustomFieldViewModel> _parent;
        private readonly string _clean;
        private string _dirty;
        private bool _canAdd;

        public SettingsCustomFieldViewModel(ICollection<SettingsCustomFieldViewModel> parent, string clean)
        {
            _parent = parent ?? throw new ArgumentNullException(nameof(parent));
            _clean = clean;
            Value = clean;
        }

        public bool CanAdd => _canAdd;

        public bool CanRemove => _parent.Contains(this);

        public string Value
        {
            get => _dirty;
            set {
                this.RaiseAndSetIfChanged(ref _dirty, value);
                this.RaiseAndSetIfChanged(
                    ref _canAdd, 
                    !string.IsNullOrEmpty(value) && !_parent.Any(vm => vm.Value == value),
                    nameof(CanAdd));
            }
        }

        public void Add()
        {
            _parent.Add(new SettingsCustomFieldViewModel(_parent, Value));
            Value = string.Empty;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SettingsCustomFieldViewModel);
        }

        public bool Equals(SettingsCustomFieldViewModel other)
        {
            return other != null &&
                   _clean == other._clean;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_clean);
        }

        public void Remove()
        {
            _parent.Remove(this);
        }
    }
}