using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using KiCadDbLib.ReactiveForms.Validation;
using Newtonsoft.Json.Linq;
using ReactiveUI;

namespace KiCadDbLib.ReactiveForms
{
    public class FormControl : AbstractControl, INotifyDataErrorInfo
    {
        private readonly ObservableAsPropertyHelper<bool> _hasErrorProperty;
        private readonly string _initialState;
        private readonly ObservableAsPropertyHelper<bool> _isDirtyProperty;
        private readonly ObservableAsPropertyHelper<bool> _isRequiredProperty;
        private IEnumerable<string> _errors;
        private IValidator _validator;
        private string _value;
        public FormControl(string initialState = null)
        {
            _initialState = initialState;
            _value = initialState;

            _errors = Enumerable.Empty<string>();

            _isRequiredProperty = this.WhenAnyValue(vm => vm.Validator)
                .Where(validator => validator != null)
                .Select(validator => Validators.IsRequired(validator))
                .ToProperty(this, nameof(IsRequired));

            _isDirtyProperty = this.WhenAnyValue(vm => vm.Value)
                .Select(value => !(_initialState?.Equals(value, StringComparison.Ordinal) == true))
                .ToProperty(this, nameof(IsDirty));

            _hasErrorProperty = this.WhenAnyValue(vm => vm.Value, vm => vm.IsRequired)
                .Where(changes => changes.Item1 != null)
                .Select(_ => Validate())
                .ToProperty(this, nameof(HasErrors));

            this.WhenAnyValue(vm => vm.HasErrors)
                .Subscribe(_ => ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Value))));
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool HasErrors => _hasErrorProperty.Value;
        public override bool IsDirty => _isDirtyProperty.Value;
        public bool IsRequired => _isRequiredProperty.Value;

        public IValidator Validator
        {
            get => _validator;
            set => this.RaiseAndSetIfChanged(ref _validator, value);
        }

        public string Value
        {
            get => _value;
            set => this.RaiseAndSetIfChanged(ref _value, value);
        }

        public IEnumerable GetErrors(string propertyName)
        {
            return propertyName.Equals(nameof(Value), StringComparison.OrdinalIgnoreCase)
                ? _errors
                : Enumerable.Empty<string>();
        }

        private bool Validate()
        {
            _errors = (Validator ?? Validators.None).Validate(this).ToArray();
            return _errors.Any();
        }

        public override JToken GetValue()
        {
            return new JValue(Value);
        }
    }
}