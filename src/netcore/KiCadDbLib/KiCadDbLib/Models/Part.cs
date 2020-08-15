using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace KiCadDbLib.Models
{
    public class Part
    {
        [JsonConstructor]
        public Part()
        {
            CustomFields = new Dictionary<string, string>();
        }

        /// <summary>
        /// Gets a dictionary of the custom fields with it's values.
        /// </summary>
        public Dictionary<string, string> CustomFields { get; }

        /// <summary>
        /// Gets or sets the datasheet location.
        /// </summary>
        public string Datasheet { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the footprint reference.
        /// </summary>
        public string Footprint { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the keywords.
        /// </summary>
        public string Keywords { get; set; }

        /// <summary>
        /// Gets or sets the library the <see cref="Part"/> belongs to.
        /// </summary>
        public string Library { get; set; }

        /// <summary>
        /// Gets or sets the reference (R, L, C etc.)
        /// </summary>
        public string Reference { get; set; }

        /// <summary>
        /// Gets or sets the symbol reference.
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public string Value { get; set; }

    }
}
