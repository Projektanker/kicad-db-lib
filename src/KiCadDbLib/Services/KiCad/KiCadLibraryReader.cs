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

            return Directory.EnumerateDirectories(directory, $"*{FileExtensions.Pretty}")
                .SelectMany(GetFootprintInfos)
                .ToArray();
        }

        public static async Task<LibraryItemInfo[]> GetSymbolInfosFromDirectoryAsync(string directory)
        {
            if (!Directory.Exists(directory))
            {
                throw new DirectoryNotFoundException($"Directory \"{directory}\" not found.");
            }

            return await Directory.EnumerateFiles(directory, $"*{FileExtensions.Lib}")
                .ToAsyncEnumerable()
                .SelectMany(GetSymbolInfosAsync)
                .ToArrayAsync()
                .ConfigureAwait(false);
        }

        public Task<string[]> GetSymbolAsync(string directory, LibraryItemInfo symbolInfo, bool bufferLibrary = true)
        {
            return GetSymbolAsync(
                libraryFilePath: Path.Combine(directory, symbolInfo.Library + FileExtensions.Lib),
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

            var result = new string[symbolLines.Length + 3];

            // Add comment before symbol
            result[0] = $"# {symbolName}";
            result[1] = "#";

            // Copy symbol to result array
            symbolLines.CopyTo(result, 2);

            // Add comment after symbol
            result[^1] = "#";
            return result;
        }

        private static LibraryItemInfo[] GetFootprintInfos(string directory)
        {
            var library = new DirectoryInfo(directory).Name[..^FileExtensions.Pretty.Length];

            return Directory.EnumerateFiles(directory, $"*{FileExtensions.KicadMod}")
               .Select(Path.GetFileNameWithoutExtension)
               .Select(footprint => new LibraryItemInfo(library, footprint))
               .ToArray();
        }

        private static async IAsyncEnumerable<LibraryItemInfo> GetSymbolInfosAsync(string libraryFilePath)
        {
            var library = Path.GetFileNameWithoutExtension(libraryFilePath);

            var symbolRegex = new Regex("DEF ([^ ]+)");

            var lines = await File.ReadAllLinesAsync(libraryFilePath, Encoding.UTF8)
                .ConfigureAwait(false);

            var symbols = lines
                .Select(line => symbolRegex.Match(line))
                .Where(match => match.Success)
                .Select(match => match.Groups[1].Value)
                .Select(symbolName => new LibraryItemInfo(library, symbolName));

            foreach (var symbol in symbols)
            {
                yield return symbol;
            }
        }
    }
}