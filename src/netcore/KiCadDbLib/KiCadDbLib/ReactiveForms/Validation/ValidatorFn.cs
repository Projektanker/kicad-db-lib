using System;
using System.Collections.Generic;
using System.Text;

namespace KiCadDbLib.ReactiveForms.Validation
{
    class ValidatorFn : IValidator
    {
        private readonly Func<FormControl, IEnumerable<string>> _validatorFn;

        public ValidatorFn(Func<FormControl, IEnumerable<string>> validatorFn)
        {
            _validatorFn = validatorFn ?? throw new ArgumentNullException(nameof(validatorFn));
        }

        public IEnumerable<string> Validate(FormControl control)
        {
            return _validatorFn(control);
        }
    }
}
