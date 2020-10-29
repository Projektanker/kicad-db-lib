using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;

namespace KiCadDbLib.Controls
{
    public class LabeledAutoCompleteBox : LabeledTextBox
    {
        public static readonly DirectProperty<LabeledAutoCompleteBox, Func<string, CancellationToken, Task<IEnumerable<object>>>> AsyncPopulatorProperty =
            AvaloniaProperty.RegisterDirect<LabeledAutoCompleteBox, Func<string, CancellationToken, Task<IEnumerable<object>>>>(
                nameof(AsyncPopulator),
                labeledAutoCompleteBox => labeledAutoCompleteBox.AsyncPopulator,
                (labeledAutoCompleteBox, value) => labeledAutoCompleteBox.AsyncPopulator = value);

        private Func<string, CancellationToken, Task<IEnumerable<object>>> _asyncPopulator;

        public Func<string, CancellationToken, Task<IEnumerable<object>>> AsyncPopulator
        {
            get => _asyncPopulator;
            set => SetAndRaise(AsyncPopulatorProperty, ref _asyncPopulator, value);
        }
    }
}