using System.Collections.Generic;
using ReactiveUI;

namespace KiCadDbLib.ReactiveForms
{
    public class AutoCompleteFormControl : FormControl
    {
        private IEnumerable<string> _items;

        public AutoCompleteFormControl()
        {
        }

        public AutoCompleteFormControl(string initialState)
            : base(initialState)
        {
        }

        public IEnumerable<string> Items
        {
            get => _items;
            set => this.RaiseAndSetIfChanged(ref _items, value);
        }
    }
}