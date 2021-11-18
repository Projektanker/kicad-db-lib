using System;

namespace KiCadDbLib.Services.KiCad
{
    public record LibraryItemInfo(string Library, string Name)
    {
        private const char _separator = ':';

        public static LibraryItemInfo Parse(string value)
        {
            var parts = value.Split(':');
            if (parts.Length != 2)
            {
                throw new ArgumentException($"Expected \"{{Library}}:{{Item}}\" but got \"{value}\"");
            }

            return new LibraryItemInfo(parts[0], parts[1]);
        }

        public override string ToString()
        {
            return string.Concat(Library, _separator, Name);
        }
    }
}