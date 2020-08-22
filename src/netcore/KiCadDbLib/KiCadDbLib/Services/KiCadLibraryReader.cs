using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KiCadDbLib.Services
{
    class KiCadLibraryReader
    {
        private const string _endDef = "ENDDEF";

        private string _getSymbolLibrary;
        private string[] _getSymbolLines;

        public async Task<string[]> GetFootprintsFromDirectoryAsync(string directory)
        {
            if (!Directory.Exists(directory))
            {
                throw new DirectoryNotFoundException($"Directory \"{directory}\" not found.");
            }

            List<string> result = new List<string>();
            foreach (string prettyDir in Directory.EnumerateDirectories(directory, "*.pretty"))
            {
                string prettyName = Path.GetFileNameWithoutExtension(prettyDir);
                string[] footprints = await GetFootprintsAsync(prettyDir);
                result.AddRange(footprints.Select(footprint => string.Join(':', prettyName, footprint)));
            }

            return result.ToArray();
        }

        public async Task<string> GetSymbolAsync(string library, string symbol, bool bufferLibrary = true)
        {
            if (!File.Exists(library))
            {
                throw new FileNotFoundException($"Library \"{library}\" not found.");
            }

            string[] lines;
            if (bufferLibrary && _getSymbolLibrary.Equals(library, StringComparison.Ordinal))
            {
                lines = _getSymbolLines;
            }
            else
            {
                lines = await File.ReadAllLinesAsync(library, Encoding.UTF8);
                _getSymbolLibrary = bufferLibrary ? library : null;
                _getSymbolLines = bufferLibrary ? lines : null;
            }


            // Search begin of symbol (DEF ...)            
            string[] start = lines
                .SkipWhile(line => !line.StartsWith($"DEF {symbol}", StringComparison.Ordinal))
                .ToArray();

            if (start.Length == 0)
            {
                throw new KeyNotFoundException($"Symbol \"{symbol}\" not found in library \"{library}\".");
            }

            // Search end of symbol (ENDDEF)
            string[] symbolLines = start
                .TakeWhile(line => !line.StartsWith(_endDef, StringComparison.Ordinal))
                .ToArray();

            if (!symbolLines[^1].StartsWith(_endDef, StringComparison.Ordinal))
            {
                throw new Exception($"Symbol library \"{library}\" is corrupted. \"{_endDef}\" not found.");
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

        public async Task<string[]> GetSymbolsFromDirectoryAsync(string directory)
        {
            if (!Directory.Exists(directory))
            {
                throw new DirectoryNotFoundException($"Directory \"{directory}\" not found.");
            }

            List<string> result = new List<string>();
            foreach (string libFile in Directory.EnumerateFiles(directory, "*.lib"))
            {
                string libraryName = Path.GetFileNameWithoutExtension(libFile);
                string[] symbols = await GetSymbolsAsync(libFile);
                result.AddRange(symbols.Select(symbol => string.Join(':', libraryName, symbol)));
            }

            return result.ToArray();
        }

        private async Task<string[]> GetFootprintsAsync(string directory)
        {
            if (!Directory.Exists(directory))
            {
                throw new DirectoryNotFoundException($"Directory \"{directory}\" not found.");
            }

            return await Task.Run(() =>
            {
                return Directory.EnumerateFiles(directory, "*.kicad_mod")
                   .Select(file => Path.GetFileNameWithoutExtension(file))
                   .ToArray();
            });
        }

        private async Task<string[]> GetSymbolsAsync(string library)
        {
            if (!File.Exists(library))
            {
                throw new FileNotFoundException($"Library \"{library}\" not found.");
            }

            Regex symbolRegex = new Regex("DEF ([^ ]+)");

            return await Task.Run(() =>
            {
                var symbols = File.ReadLines(library, Encoding.UTF8)
                    .Select(line => symbolRegex.Match(line))
                    .Where(match => match.Success)
                    .Select(match => match.Groups[1].Value);

                return symbols.ToArray();
            });
        }
    }
}
