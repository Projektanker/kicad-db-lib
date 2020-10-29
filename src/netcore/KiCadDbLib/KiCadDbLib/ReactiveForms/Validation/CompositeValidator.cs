using System;
using System.Collections.Generic;
using System.Linq;

namespace KiCadDbLib.ReactiveForms.Validation
{
    internal class CompositeValidator : IValidator
    {
        public CompositeValidator(IEnumerable<IValidator> validators)
        {
            Validators = validators ?? throw new ArgumentNullException(nameof(validators));
        }

        public IEnumerable<IValidator> Validators { get; }

        /// <inheritdoc/>
        public IEnumerable<string> Validate(FormControl control)
        {
            return Validators.SelectMany(v => v.Validate(control));
        }
    }
}