using System;
using System.Collections.Generic;
using System.Text;

namespace KiCadDbLib.ReactiveForms.Validation
{
    public class ValidatorFn : IValidator
    {
        private readonly Func<FormControl, IEnumerable<string>> _validatorFn;

        public ValidatorFn(Func<FormControl, IEnumerable<string>> validatorFn)
        {
            _validatorFn = validatorFn ?? throw new ArgumentNullException(nameof(validatorFn));
        }

        /// <inheritdoc/>
        public IEnumerable<string> Validate(FormControl control)
        {
            return _validatorFn(control);
        }
    }
}