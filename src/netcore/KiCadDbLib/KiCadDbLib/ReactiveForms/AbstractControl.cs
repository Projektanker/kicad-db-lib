using System;
using System.Collections;
using System.ComponentModel;
using System.Reactive.Linq;
using Newtonsoft.Json.Linq;
using ReactiveUI;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Contexts;
using ReactiveUI.Validation.Extensions;

namespace KiCadDbLib.ReactiveForms
{
    public abstract class AbstractControl : ReactiveObject
    {

        private string _label;

        protected AbstractControl()
        {

        }

        public abstract bool HasErrors { get; }

        public abstract bool IsDirty { get; }

        public string Label
        {
            get => _label;
            set => this.RaiseAndSetIfChanged(ref _label, value);
        }
        public abstract JToken GetValue();

        public abstract bool Validate();

        public T ToObject<T>()
        {
            return GetValue().ToObject<T>();
        }
    }
}