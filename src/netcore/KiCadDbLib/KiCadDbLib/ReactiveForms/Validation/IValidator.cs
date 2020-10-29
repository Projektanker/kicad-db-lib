using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KiCadDbLib.ReactiveForms.Validation
{
    public interface IValidator
    {
        IEnumerable<string> Validate(FormControl control);
    }
}