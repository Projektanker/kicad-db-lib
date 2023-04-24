using System.Collections.Generic;

namespace KiCadDbLib.Models
{
    public class Part
    {
        public int Id { get; set; }
        public string Library { get; set; } = string.Empty;
        public string Reference { get; set; } = string.Empty;
        public string Symbol { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string Footprint { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Datasheet { get; set; } = string.Empty;
        public string Keywords { get; set; } = string.Empty;

        public IReadOnlyDictionary<string, string> CustomFields { get; set; }
            = new Dictionary<string, string>();

        public override string ToString()
        {
            return Id.ToString();
        }
    }
}