using System.Linq;
using Newtonsoft.Json.Linq;
using Projektanker.Collections;

namespace KiCadDbLib.ReactiveForms
{
    public class FormGroup : AbstractControl
    {
        public FormGroup()
        {
            Controls = new OrderedDictionary<string, AbstractControl>();
        }

        public OrderedDictionary<string, AbstractControl> Controls { get; }

        public override bool IsDirty => Controls.Values.Any(control => control.IsDirty);

        public override JToken GetValue()
        {
            return new JObject(
                Controls.Select(kv => new JProperty(kv.Key, kv.Value.GetValue())));
        }
    }
}