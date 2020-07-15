﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using FontAwesome.Avalonia.Shared;
using Newtonsoft.Json;

namespace FontAwesome.Avalonia.FontAwesome
{
    internal static class FontAwesomeIconProvider
    {
        private static readonly Lazy<Dictionary<string, FontAwesomeIcon>> _lazyIcons = new Lazy<Dictionary<string, FontAwesomeIcon>>(Parse);
        private const string _faKeyPrefix = "fa-";
        private const string _resource = "FontAwesome.Avalonia.Assets.FontAwesome.icons.json";

        private static Dictionary<string, FontAwesomeIcon> Icons => _lazyIcons.Value;

        private static Dictionary<string, FontAwesomeIcon> Parse()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(_resource))
            using (TextReader textReader = new StreamReader(stream))
            using (JsonReader jsonReader = new JsonTextReader(textReader))
            {
                JsonSerializer serializer = JsonSerializer.Create();
                var result = serializer.Deserialize<Dictionary<string, FontAwesomeIcon>>(jsonReader);
                return result;
            }
        }

        internal static string GetIconPath(string value)
        {
            string[] splitted = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string key;
            Style? style;
            if (splitted.Length == 1)
            {
                key = splitted[0];
                style = null;
            }
            else if (splitted.Length == 2)
            {
                key = splitted[1];

                StylePrefix stylePrefix = Enum.Parse<StylePrefix>(splitted[0], ignoreCase: true);
                style = stylePrefix.ToStyle();
            }
            else
            {
                throw new ArgumentException(nameof(value));
            }

            // Remove "fa-" substring
            key = key.Substring(_faKeyPrefix.Length);

            if (!Icons.TryGetValue(key, out FontAwesomeIcon icon))
            {
                throw new KeyNotFoundException($"Icon \"{key}\" not found!");
            }
            else if (!style.HasValue)
            {
                return icon.Svg.Values.First().Path;
            }
            else if (icon.Svg.TryGetValue(style.Value, out Svg svg))
            {
                return svg.Path;
            }
            else
            {
                throw new KeyNotFoundException($"Style \"{style}\" not found!");
            }
        }
    }
}