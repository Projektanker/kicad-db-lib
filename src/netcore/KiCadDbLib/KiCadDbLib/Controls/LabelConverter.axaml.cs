using Avalonia.Data.Converters;

namespace KiCadDbLib.Controls
{
    public class LabelConverter : FuncValueConverter<ReactiveForms.FormControl, string>
    {
        public LabelConverter()
            : base(GetLabel)
        {
        }

        private static string GetLabel(ReactiveForms.FormControl input)
        {
            return input.Label + (input.IsRequired ? "*" : string.Empty);
        }
    }
}