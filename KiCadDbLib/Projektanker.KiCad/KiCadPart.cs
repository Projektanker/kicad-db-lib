using System.Collections.Generic;

namespace Projektanker.KiCad
{
    public class KiCadPart
    {
        public string Identifier { get; set; }

        /// <summary>
        /// Schematic reference. E.g. "R" for resistors
        /// </summary>
        public string Reference { get; set; }

        /// <summary>
        /// Part value. E.g. "10K". Must be unique per <see cref="Library"/>
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Footprint of part. Have to match KiCad footprint libraries. E.g. "Resistor_SMD:R_0603_1608Metric"
        /// </summary>
        public string Footprint { get; set; }

        /// <summary>
        /// Reference to symbol. ../lib1.lib:symbol1
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Library the part have be stored in. <see cref="Value"/> must be unique per library.
        /// </summary>
        public string Library { get; set; }

        /// <summary>
        /// Datasheet link
        /// </summary>
        public string Datasheet { get; set; }

        /// <summary>
        /// Description of part.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Keywords to find part
        /// </summary>
        public string Keywords { get; set; }

        /// <summary>
        /// Custom fields like manufacturer etc.
        /// </summary>
        public Dictionary<string, string> CustomFields { get; set; }
    }
}