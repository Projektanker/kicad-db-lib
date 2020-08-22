using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace KiCadDbLib.ReactiveForms
{
    public class FormGroup : AbstractControl
    {
        private readonly IList<FormGroupControl> _controls;

        public FormGroup(string label = default)
        {
            _controls = new List<FormGroupControl>();
            Label = label;
        }

        public IEnumerable<FormGroupControl> Controls => _controls;

        public override bool IsDirty => Controls
            .Select(item => item.Control)
            .Any(control => control.IsDirty);

        public override bool HasErrors => Controls
            .Select(item => item.Control)
            .Any(control => control.HasErrors);

        public bool Add(string name, AbstractControl control)
        {
            if (Contains(name))
            {
                return false;
            }
            else
            {
                _controls.Add(new FormGroupControl(name, control));
                return true;
            }
        }

        public bool Contains(string name)
        {
            return _controls.Any(item => item.Name.Equals(name, StringComparison.Ordinal));
        }

        public override JToken GetValue()
        {
            return new JObject(
                Controls.Select(item => new JProperty(item.Name, item.Control.GetValue())));
        }

        public bool Remove(string name, out AbstractControl control)
        {
            var item = _controls.FirstOrDefault(item => item.Name.Equals(name, StringComparison.Ordinal));

            if (item == default)
            {
                control = default;
                return false;
            }
            else
            {
                control = item.Control;
                _controls.Remove(item);
                return true;
            }
        }

        public override bool Validate()
        {
            bool[] areValid = Controls.Select(item => item.Control)
                .Select(control => control.Validate())
                .ToArray();

            return areValid.All(isValid => isValid);
        }
    }
}