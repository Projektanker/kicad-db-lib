using Newtonsoft.Json.Linq;
using ReactiveUI;

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

        public T GetValue<T>()
        {
            return GetValue().ToObject<T>();
        }

        public abstract bool Validate();
    }
}