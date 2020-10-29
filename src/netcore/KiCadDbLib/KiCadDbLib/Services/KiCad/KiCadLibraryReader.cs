using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static MoreLinq.Extensions.TakeUntilExtension;

namespace KiCadDbLib.Services.KiCad
{
    internal class KiCadLibraryReader
    {
        private const string _endDef = "ENDDEF";

        private string _getSymbolLibrary;
        private string[] _getSymbolLines;

        public static async Task<LibraryItemInfo[]> GetFootprintInfosFromDirectoryAsync(string directory)
        {
            if (!Directory.Exists(directory))
            {
                throw new DirectoryNotFoundException($"Directory \"{directory}\" not found.");
            }

            List<LibraryItemInfo> result = new List<LibraryItemInfo>();
            foreach (string footprintDirectory in Directory.EnumerateDirectories(directory, $"*{FileExtensions.Pretty}"))
            {
                LibraryItemInfo[] footprints = await GetFootprintInfosAsync(footprintDirectory).ConfigureAwait(false);
                result.AddRange(footprints);
            }

            return result.ToArray();
        }

        public static async Task<LibraryItemInfo[]> GetSymbolInfosFromDirectoryAsync(string directory)
        {
            if (!Directory.Exists(directory))
            {
                throw new DirectoryNotFoundException($"Directory \"{directory}\" not found.");
            }

            List<LibraryItemInfo> result = new List<LibraryItemInfo>();
            foreach (string libFile in Directory.EnumerateFiles(directory, $"*{FileExtensions.Lib}"))
            {
                LibraryItemInfo[] symbols = await GetSymbolInfosAsync(libFile).ConfigureAwait(false);
                result.AddRange(symbols);
            }

            return result.ToArray();
        }

        public Task<string[]> GetSymbolAsync(LibraryItemInfo symbolInfo, bool bufferLibrary = true)
        {
            return GetSymbolAsync(
                libraryFilePath: symbolInfo.Library,
                symbolName: symbolInfo.Name,
                bufferLibrary: bufferLibrary);
        }

        public async Task<string[]> GetSymbolAsync(string libraryFilePath, string symbolName, bool bufferLibrary = true)
        {
            if (!File.Exists(libraryFilePath))
            {
                throw new FileNotFoundException($"Library \"{libraryFilePath}\" not found.");
            }

            string[] lines;
            if (bufferLibrary && (_getSymbolLibrary?.Equals(libraryFilePath, StringComparison.Ordinal) ?? false))
            {
                lines = _getSymbolLines;
            }
            else
            {
                lines = await File.ReadAllLinesAsync(libraryFilePath, Encoding.UTF8).ConfigureAwait(false);
                _getSymbolLibrary = bufferLibrary ? libraryFilePath : null;
                _getSymbolLines = bufferLibrary ? lines : null;
            }

            // Search begin of symbol (DEF ...)
            var start = lines
                .SkipWhile(line => !line.StartsWith($"DEF {symbolName}", StringComparison.Ordinal));

            if (!start.Any())
            {
                throw new KeyNotFoundException($"Symbol \"{symbolName}\" not found in library \"{libraryFilePath}\".");
            }

            // Search end of symbol (ENDDEF)
            var symbolLines = start
                .TakeUntil(line => line.StartsWith(_endDef, StringComparison.Ordinal))
                .ToArray();

            if (!symbolLines[^1].StartsWith(_endDef, StringComparison.Ordinal))
            {
                throw new FormatException($"Symbol library \"{libraryFilePath}\" is corrupted. \"{_endDef}\" not found.");
            }

            string[] result = new string[symbolLines.Length + 3];

            // Add comment before symbol
            result[0] = $"# {symbolName}";
            result[1] = "#";

            // Copy symbol to result array
            symbolLines.CopyTo(result, 2);

            // Add comment after symbol
            result[^1] = "#";
            return result;
        }

        private static async Task<LibraryItemInfo[]> GetFootprintInfosAsync(string directory)
        {
            if (!Directory.Exists(directory))
            {
                throw new DirectoryNotFoundException($"Directory \"{directory}\" not found.");
            }

            return await Task.Run(() =>
            {
                return Directory.EnumerateFiles(directory, $"*{FileExtensions.KicadMod}")
                   .Select(file => Path.GetFileNameWithoutExtension(file))
                   .Select(footprint => new LibraryItemInfo() { Library = directory, Name = footprint })
                   .ToArray();
            }).ConfigureAwait(false);
        }

        private static async Task<LibraryItemInfo[]> GetSymbolInfosAsync(string libraryFilePath)
        {
            if (!File.Exists(libraryFilePath))
            {
                throw new FileNotFoundException($"Library \"{libraryFilePath}\" not found.");
            }

            Regex symbolRegex = new Regex("DEF ([^ ]+)");

            return await Task.Run(() =>
            {
                var symbols = File.ReadLines(libraryFilePath, Encoding.UTF8)
                    .Select(line => symbolRegex.Match(line))
                    .Where(match => match.Success)
                    .Select(match => match.Groups[1].Value)
                    .Select(symbolName => new LibraryItemInfo() { Library = libraryFilePath, Name = symbolName });

                return symbols.ToArray();
            }).ConfigureAwait(false);
        }
    }
}