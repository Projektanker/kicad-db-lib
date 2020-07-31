using System;
using System.Collections.Generic;
using System.Text;

namespace KiCadDbLib.Models
{
    public class Part
    {
        public Part()
        {
            CustomFields = new Dictionary<string, string>();
        }

        public string Id { get; set; }

        public string Library { get; set; }

        public string Reference { get; set; }

        public Dictionary<string, string> CustomFields { get; }
    }
}
