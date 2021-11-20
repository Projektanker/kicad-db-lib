using ReactiveUI;

namespace KiCadDbLib.Models
{
    public class CustomField : ReactiveObject
    {
        private string _name;
        private string _value;

        public CustomField(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        public string Value
        {
            get => _value;
            set => this.RaiseAndSetIfChanged(ref _value, value);
        }
    }
}
