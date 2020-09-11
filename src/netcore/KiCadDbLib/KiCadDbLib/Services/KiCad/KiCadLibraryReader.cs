using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Projektanker.Extensions;

namespace KiCadDbLib.Services.KiCad
{
    class KiCadLibraryReader
    {
        private const string _endDef = "ENDDEF";

        private string _getSymbolLibrary;
        private string[] _getSymbolLines;

        public async Task<LibraryItemInfo[]> GetFootprintsFromDirectoryAsync(string directory)
        {
            if (!Directory.Exists(directory))
            {
                throw new DirectoryNotFoundException($"Directory \"{directory}\" not found.");
            }

            List<LibraryItemInfo> result = new List<LibraryItemInfo>();
            foreach (string footprintDirectory in Directory.EnumerateDirectories(directory, $"*{FileExtensions.Pretty}"))
            {
                LibraryItemInfo[] footprints = await GetFootprintsAsync(footprintDirectory);
                result.AddRange(footprints);
            }

            return result.ToArray();
        }

        public Task<string[]> GetSymbolAsync(string libraryDirectory, LibraryItemInfo symbolInfo, bool bufferLibrary = true)
        {
            return GetSymbolAsync(
                libraryFilePath: Path.Combine(libraryDirectory, symbolInfo.Library + FileExtensions.Lib),
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
            if (bufferLibrary && _getSymbolLibrary.Equals(libraryFilePath, StringComparison.Ordinal))
            {
                lines = _getSymbolLines;
            }
            else
            {
                lines = await File.ReadAllLinesAsync(libraryFilePath, Encoding.UTF8);
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
                .TakeWhile(line => !line.StartsWith(_endDef, StringComparison.Ordinal))
                .ToArray();

            if (!symbolLines[^1].StartsWith(_endDef, StringComparison.Ordinal))
            {
                throw new Exception($"Symbol library \"{libraryFilePath}\" is corrupted. \"{_endDef}\" not found.");
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



        public async Task<LibraryItemInfo[]> GetSymbolsFromDirectoryAsync(string directory)
        {
            if (!Directory.Exists(directory))
            {
                throw new DirectoryNotFoundException($"Directory \"{directory}\" not found.");
            }

            List<LibraryItemInfo> result = new List<LibraryItemInfo>();
            foreach (string libFile in Directory.EnumerateFiles(directory, $"*{FileExtensions.Lib}"))
            {
                LibraryItemInfo[] symbols = await GetSymbolsAsync(libFile);
                result.AddRange(symbols);
            }

            return result.ToArray();
        }

        private async Task<LibraryItemInfo[]> GetFootprintsAsync(string directory)
        {
            if (!Directory.Exists(directory))
            {
                throw new DirectoryNotFoundException($"Directory \"{directory}\" not found.");
            }

            return await Task.Run(() =>
            {
                string libraryName = Path.GetFileNameWithoutExtension(directory);
                return Directory.EnumerateFiles(directory, $"*{FileExtensions.KicadMod}")
                   .Select(file => Path.GetFileNameWithoutExtension(file))
                   .Select(footprint => new LibraryItemInfo() { Library = libraryName, Name = footprint })
                   .ToArray();
            });
        }

        private async Task<LibraryItemInfo[]> GetSymbolsAsync(string libraryFilePath)
        {
            if (!File.Exists(libraryFilePath))
            {
                throw new FileNotFoundException($"Library \"{libraryFilePath}\" not found.");
            }

            Regex symbolRegex = new Regex("DEF ([^ ]+)");

            return await Task.Run(() =>
            {
                string libraryName = Path.GetFileNameWithoutExtension(libraryFilePath);

                var symbols = File.ReadLines(libraryFilePath, Encoding.UTF8)
                    .Select(line => symbolRegex.Match(line))
                    .Where(match => match.Success)
                    .Select(match => match.Groups[1].Value)
                    .Select(symbolName => new LibraryItemInfo() { Library = libraryName, Name = symbolName });

                return symbols.ToArray();
            });
        }
    }
}
