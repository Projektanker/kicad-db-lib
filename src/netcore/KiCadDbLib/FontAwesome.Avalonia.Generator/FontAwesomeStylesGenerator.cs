using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace FontAwesome.Avalonia.Generator
{
    public static class FontAwesomeStylesGenerator
    {
        private static readonly string iconsYamlAddress = "https://raw.githubusercontent.com/FortAwesome/Font-Awesome/master/metadata/icons.yml";

        public static void Generate(StringBuilder output)
        {
            Generate(() => new StylesGenerator(output));
        }

        public static string Generate()
        {
            StringBuilder sb = new StringBuilder();
            Generate(() => new StylesGenerator(sb));
            return sb.ToString();
        }

        private static void Generate(Func<StylesGenerator> stylesGeneratorFactory)
        {
            IEnumerable<Icon> icons = GetIcons();
            using (StylesGenerator generator = stylesGeneratorFactory.Invoke())
            {
                generator.AddStyle("Button.fa", (property: "FontFamily", value: "avares://FontAwesome.Avalonia/Fonts#Font Awesome 5 Free"));
                //generator.AddStyle("Button.fa", 
                //    (property: "FontFamily", value: "resm:FontAwesome.Avalonia.Fonts.Font Awesome 5 Free-Solid-900.otf?assembly=FontAwesome.Avalonia#Font Awesome 5 Free"));

                foreach (Icon icon in icons)
                {
                    WriteIconStyle(generator, icon);
                }

                generator.Done();
            }
        }

        private static IEnumerable<Icon> GetIcons()
        {
            DeserializerBuilder deserializerBuilder = new DeserializerBuilder();
            IDeserializer deserializer = deserializerBuilder
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .IgnoreUnmatchedProperties()
                .Build();

            using (var client = new WebClient())
            using (Stream stream = client.OpenRead(iconsYamlAddress))
            using (TextReader textReader = new StreamReader(stream))
            {
                Dictionary<string, Icon> iconsByLabel = deserializer.Deserialize<Dictionary<string, Icon>>(textReader);
                return iconsByLabel.Values.OrderBy(i => i.Label);
            }
        }

        private static void WriteIconStyle(StylesGenerator generator, Icon icon)
        {
            string label = icon.Label
                .Replace(' ', '_')
                .Replace('-', '_')
                .ToLowerInvariant();
            Regex regex = new Regex("[^a-z_]");
            label = regex.Replace(label, string.Empty);
            string selector = $"Button.fa_{label}";
            int utf32 = int.Parse(icon.Unicode, NumberStyles.HexNumber);
            string content = char.ConvertFromUtf32(utf32);

            generator.AddStyle(selector, (property: "Content", value: content));
        }
    }
}