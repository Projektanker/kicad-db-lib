using System;

namespace KiCadDbLib.Services.KiCad
{
    public class LibraryItemInfo
    {
        private const char _separator = ':';

        /// <summary>
        /// Gets or sets the name of the item.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the library.
        /// </summary>
        public string Library { get; set; }

        public static LibraryItemInfo Parse(string value)
        {
            int lastIndex = value.LastIndexOf(_separator);
            if (lastIndex < 1)
            {
                throw new ArgumentException($"Expected \"Library:Item\" but got \"{value}\"");
            }

            return new LibraryItemInfo
            {
                Library = value.Substring(0, lastIndex),
                Name = value.Substring(lastIndex + 1),
            };
        }

        public static string ToString(string library, string item)
        {
            return string.Join(_separator, library, item);
        }

        public override string ToString()
        {
            return string.Join(_separator, Library, Name);
        }
    }
}