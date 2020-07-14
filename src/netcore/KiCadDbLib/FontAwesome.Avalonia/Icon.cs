using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace FontAwesome.Avalonia
{
    internal class Icon
    {
        //[JsonProperty("styles")]
        public List<string> Styles { get; set; }

        //[JsonProperty("unicode")]
        public string Unicode { get; set; }

        //[JsonProperty("free")]
        public List<string> Free { get; set; }

        //[JsonProperty("svg")]
        public Dictionary<string, Svg> Svg { get; set; }
    }
}
