using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;

namespace KiCadDbLib.Services.KiCad
{
    public class LibraryItemInfo
    {
        private const char _separator = ':';

        /// <summary>
        /// Gets or sets the name of the library.
        /// </summary>
        public string Library { get; set; }

        /// <summary>
        /// Gets or sets the name of the item.
        /// </summary>
        public string Item { get; set; }


        public override string ToString()
        {
            return string.Join(_separator, Library, Item);
        }

        public static LibraryItemInfo Parse(string value)
        {
            int lastIndex = value.LastIndexOf(_separator);
            if (lastIndex < 1)
            {
                throw new ArgumentException($"Expected \"Library:Name\" but got \"{value}\"");
            }
            return new LibraryItemInfo
            {
                Library = value.Substring(0, lastIndex),
                Item = value.Substring(lastIndex + 1),
            };
        }

        public static string ToString(string library, string item)
        {
            return string.Join(_separator, library, item);
        }


    }
}
