using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
            foreach (string footprintDirectory in Directory.EnumerateDirectories(directory, "*.pretty"))
            {
                LibraryItemInfo[] footprints = await GetFootprintsAsync(footprintDirectory);
                result.AddRange(footprints);
            }

            return result.ToArray();
        }

        public async Task<string> GetSymbolAsync(string libraryDirectory, LibraryItemInfo symbolInfo, bool bufferLibrary = true)
        {
            return await GetSymbolAsync(
                libraryPath: Path.Combine(libraryDirectory, $"{symbolInfo.Item}.lib"),
                symbol: symbolInfo.Item,
                bufferLibrary: bufferLibrary);
        }

        public async Task<string> GetSymbolAsync(string libraryPath, string symbol, bool bufferLibrary = true)
        {
            if (!File.Exists(libraryPath))
            {
                throw new FileNotFoundException($"Library \"{libraryPath}\" not found.");
            }

            string[] lines;
            if (bufferLibrary && _getSymbolLibrary.Equals(libraryPath, StringComparison.Ordinal))
            {
                lines = _getSymbolLines;
            }
            else
            {
                lines = await File.ReadAllLinesAsync(libraryPath, Encoding.UTF8);
                _getSymbolLibrary = bufferLibrary ? libraryPath : null;
                _getSymbolLines = bufferLibrary ? lines : null;
            }


            // Search begin of symbol (DEF ...)            
            string[] start = lines
                .SkipWhile(line => !line.StartsWith($"DEF {symbol}", StringComparison.Ordinal))
                .ToArray();

            if (start.Length == 0)
            {
                throw new KeyNotFoundException($"Symbol \"{symbol}\" not found in library \"{libraryPath}\".");
            }

            // Search end of symbol (ENDDEF)
            string[] symbolLines = start
                .TakeWhile(line => !line.StartsWith(_endDef, StringComparison.Ordinal))
                .ToArray();

            if (!symbolLines[^1].StartsWith(_endDef, StringComparison.Ordinal))
            {
                throw new Exception($"Symbol library \"{libraryPath}\" is corrupted. \"{_endDef}\" not found.");
            }

            // Return symbol
            StringBuilder sb = new StringBuilder();

            // Add comment before symbol
            sb.AppendLine($"# {symbol}").AppendLine("#");

            // Append symbol
            sb.AppendJoin(Environment.NewLine, symbolLines);

            // Add comment after symbol
            sb.AppendLine("#");

            return sb.ToString();
        }

        public async Task<LibraryItemInfo[]> GetSymbolsFromDirectoryAsync(string directory)
        {
            if (!Directory.Exists(directory))
            {
                throw new DirectoryNotFoundException($"Directory \"{directory}\" not found.");
            }

            List<LibraryItemInfo> result = new List<LibraryItemInfo>();
            foreach (string libFile in Directory.EnumerateFiles(directory, "*.lib"))
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
                return Directory.EnumerateFiles(directory, "*.kicad_mod")
                   .Select(file => Path.GetFileNameWithoutExtension(file))
                   .Select(footprint => new LibraryItemInfo() { Library = libraryName, Item = footprint})
                   .ToArray();
            });
        }

        private async Task<LibraryItemInfo[]> GetSymbolsAsync(string libraryPath)
        {
            if (!File.Exists(libraryPath))
            {
                throw new FileNotFoundException($"Library \"{libraryPath}\" not found.");
            }

            Regex symbolRegex = new Regex("DEF ([^ ]+)");

            return await Task.Run(() =>
            {
                string libraryName = Path.GetFileNameWithoutExtension(libraryPath);

                var symbols = File.ReadLines(libraryPath, Encoding.UTF8)
                    .Select(line => symbolRegex.Match(line))
                    .Where(match => match.Success)
                    .Select(match => match.Groups[1].Value)
                    .Select(symbolName => new LibraryItemInfo() { Library = libraryName, Item = symbolName });

                return symbols.ToArray();
            });
        }
    }
}
