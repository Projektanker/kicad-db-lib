using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace FontAwesome.Avalonia
{
    internal static class IconHelper
    {
        private static Lazy<Dictionary<string, Icon>> _lazyIcons = new Lazy<Dictionary<string, Icon>>(Parse);

        private static Dictionary<string, Icon> Icons => _lazyIcons.Value;

        private static Dictionary<string, Icon> Parse()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string[] names = assembly.GetManifestResourceNames();
            using (Stream stream = assembly.GetManifestResourceStream(names[0]))
                using(TextReader textReader = new StreamReader(stream))
                using(JsonReader jsonReader = new JsonTextReader(textReader))
            {                
                JsonSerializer serializer = JsonSerializer.Create();
                var result = serializer.Deserialize<Dictionary<string, Icon>>(jsonReader);
                return result;
            }
        }

        internal static string GetIconPath(string iconKey, string style = null)
        {
            if(!Icons.TryGetValue(iconKey, out Icon icon))
            {
                return string.Empty;
            }else if (string.IsNullOrEmpty(style))
            {
                return icon.Svg.Values.First().Path;
            }else if(icon.Svg.TryGetValue(style, out Svg svg))
            {
                return svg.Path;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
