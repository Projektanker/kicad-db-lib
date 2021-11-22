using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MoreLinq.Extensions.TakeUntilExtension;

namespace KiCadDbLib.Services.KiCad.LibraryWriter
{
    internal class LegacyKiCadLibrarySymbolTemplateFactory
    {
        private const string _endDef = "ENDDEF";

        private string? _getSymbolLibrary;
        private string[]? _getSymbolLines;

        public async Task<string[]> GetSymbolTemplateAsync(string directory, LibraryItemInfo symbolInfo, bool bufferLibrary = true)
        {
            var template = await GetSymbolAsync(
                libraryFilePath: Path.Combine(directory, symbolInfo.Library + FileExtensions.Lib),
                symbolName: symbolInfo.Name,
                bufferLibrary: bufferLibrary)
                .ConfigureAwait(false);

            // Remove empty lines
            var filtered = template.Where(line => !string.IsNullOrWhiteSpace(line));

            // Remove lines starting with '#'
            filtered = filtered.Where(line => !line.StartsWith('#'));

            // Remove ALIAS
            filtered = filtered.Where(line => !line.StartsWith("ALIAS", StringComparison.OrdinalIgnoreCase));

            // Remove FPLIST
            static bool IsPartOfFPList(string line, ref bool insideFPList)
            {
                if (line.StartsWith("$FPLIST", StringComparison.OrdinalIgnoreCase))
                {
                    insideFPList = true;
                    return true;
                }
                else if (line.StartsWith("$ENDFPLIST", StringComparison.OrdinalIgnoreCase))
                {
                    insideFPList = false;
                    return true;
                }
                else
                {
                    return insideFPList;
                }
            }

            bool insideFPList = false;
            filtered = filtered.Where(line => !IsPartOfFPList(line, ref insideFPList));

            // Done filtering.
            return filtered.ToArray();
        }

        private async Task<string[]> GetSymbolAsync(string libraryFilePath, string symbolName, bool bufferLibrary = true)
        {
            if (!File.Exists(libraryFilePath))
            {
                throw new FileNotFoundException($"Library \"{libraryFilePath}\" not found.");
            }

            string[]? lines;
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
    }
}