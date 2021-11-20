using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Data.Converters;
using KiCadDbLib.Models;

namespace KiCadDbLib.Converters
{
    public class PartsConverter : FuncValueConverter<IEnumerable<Part>, IEnumerable<Part>>
    {
        public PartsConverter()
            : base(ConvertParts)
        {
        }

        private static IEnumerable<Part> ConvertParts(IEnumerable<Part> input)
        {
            return input
                .Select(FormatDatasheet);
        }

        private static Part FormatDatasheet(Part input)
        {
            var split = input.Datasheet.Split(new[] { '\\', '/' }, StringSplitOptions.None);
            if (split.Length > 1)
            {
                string start = input.Datasheet.Contains('\\', StringComparison.Ordinal)
                    ? @"..\"
                    : "../";
                input.Datasheet = start + split[^1];
            }

            return input;
        }
    }
}