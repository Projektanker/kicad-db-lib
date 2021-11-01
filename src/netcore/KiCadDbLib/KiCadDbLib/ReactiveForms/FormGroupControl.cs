using System;

namespace KiCadDbLib.ReactiveForms
{
    public class FormGroupControl
    {
        public FormGroupControl(string name, AbstractControl control)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Control = control ?? throw new ArgumentNullException(nameof(control));
        }

        public AbstractControl Control { get; }

        public string Name { get; }
    }
}