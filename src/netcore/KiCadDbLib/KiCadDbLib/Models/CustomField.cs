using System;
using System.Collections.Generic;
using System.Text;

namespace KiCadDbLib.Models
{
    public class CustomField
    {
        public CustomField(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }
        public string Value { get; set; }
    }
}
