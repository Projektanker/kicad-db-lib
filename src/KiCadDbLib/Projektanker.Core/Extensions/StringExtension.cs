using System;

namespace Projektanker.Extensions
{
    public static class StringExtension
    {
        /// <summary>
        /// Splits a string at the line breaks. A line break is a line feed ("\n"), a carriage
        /// return ("\r") or a carriage return immediately followed by a line feed ("\r\n").
        /// </summary>
        /// <param name="value">The string to be split.</param>
        /// <param name="options">
        /// <see cref="StringSplitOptions.RemoveEmptyEntries"/> to omit empty lines from the array
        /// returned; or <see cref="StringSplitOptions.None"/> to include lines in the array returned.
        /// </param>
        /// <returns>An array whose elements contain the lines of the string.</returns>
        public static string[] SplitLines(this string value, StringSplitOptions options = StringSplitOptions.None)
        {
            return value.Split(new[] { "\r\n", "\r", "\n" }, options);
        }
    }
}