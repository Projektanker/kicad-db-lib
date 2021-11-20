using System.Collections.Generic;

namespace KiCadDbLib.ReactiveForms.Validation
{
    public interface IValidator
    {
        IEnumerable<string> Validate(FormControl control);
    }
}