using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ReactiveUI;

namespace KiCadDbLib.ReactiveForms.Validation
{
    public static class Validators
    {
        public static IValidator None { get; } = new ValidatorFn(_ => Sucess);
        public static IValidator Required { get; } = new ValidatorFn(RequiredCallback);
        private static IEnumerable<string> Sucess => Enumerable.Empty<string>();

        public static IValidator Compose(params IValidator[] validators)
        {
            return new CompositeValidator(validators);
        }

        public static IValidator Pattern(string pattern)
        {
            return Pattern(new Regex(pattern));
        }

        public static IValidator Pattern(Regex regex)
        {
            return new ValidatorFn(control => regex.IsMatch(control.Value ?? string.Empty) ? Sucess : Error(control, "contains invalid characters"));        
        }

        public static IValidator DirectoryExists { get; } 
            = new ValidatorFn(control => Directory.Exists(control.Value) ? Sucess : Error(control, "does not exist"));
        

        private static IEnumerable<string> Error(FormControl control, string error)
        {
            return Enumerable.Repeat($"{control.Label} {error}.", 1);
        }

        private static IEnumerable<string> RequiredCallback(FormControl control)
        {
            return string.IsNullOrEmpty(control.Value)
                ? Error(control, "is required")
                : Sucess;
        }
        
        internal static bool IsRequired(IValidator validator)
        {
            return validator == Required
                || (validator is CompositeValidator compositeValidator && compositeValidator.Validators.Any(v => IsRequired(v)));
        }
    }
}