using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KiCadDbLib.ReactiveForms.Validation
{
    class CompositeValidator: IValidator
    {
        public IEnumerable<IValidator> Validators { get; }

        public CompositeValidator(IEnumerable<IValidator> validators)
        {
            Validators = validators ?? throw new ArgumentNullException(nameof(validators));
        }

        public IEnumerable<string> Validate(FormControl control)
        {
            return Validators.SelectMany(v => v.Validate(control));
        }
    }
}
