using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projektanker.KiCad
{
    public class KiCadLibraryReader
    {
        private string _getSymbol_library;
        private string[] _getSymbol_lines;

        public async Task<string> GetSymbolAsync(string library, string symbol, bool bufferLibrary = true)
        {
            if (!File.Exists(library))
            {
                throw new FileNotFoundException($"Symbol library \"{library}\" not found!");
            }
            string[] lines;

            if (bufferLibrary && (library == _getSymbol_library))
            {
                lines = _getSymbol_lines;
            }
            else
            {
                lines = await File.ReadAllLinesAsync(library);
                _getSymbol_library = bufferLibrary ? library : null;
                _getSymbol_lines = bufferLibrary ? lines : null;
            }

            int start = 0, end = 0;
            // Search begin of symbol (DEF ...)
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                if (!line.StartsWith($"DEF {symbol}")) continue;

                start = i;
                break;
            }

            if (start == 0)
            {
                throw new KeyNotFoundException($"Symbol \"{symbol}\" not found in symbol library \"{library}\"");
            }
            // Search end of symbol (ENDDEF)
            for (int i = start + 1; i < lines.Length; i++)
            {
                var line = lines[i];
                if (!line.StartsWith("ENDDEF")) continue;

                end = i + 1;
                break;
            }

            if (end < start)
            {
                throw new Exception($"Symbol library \"{library}\" is corrupted! \"ENDDEF\" not found!");
            }

            // Return symbol
            StringBuilder stringBuilder = new StringBuilder();
            // Add comment before symbol
            stringBuilder.AppendLine($"# {symbol}");
            stringBuilder.AppendLine("#");

            for (int i = start; i < end; i++)
            {
                stringBuilder.AppendLine(lines[i]);
            }
            // Add comment after symbol
            stringBuilder.AppendLine("#");

            return stringBuilder.ToString();
        }

        public async Task<string> GetSymbolTemplateAsync(KiCadPart part)
        {
            var split = part.Symbol.Split(":").ToList();
            if (split.Count < 2)
            {
                throw new FormatException("Symbol must be defined as \"path:symbol\"");
            }

            string symbol = split[split.Count-1];
            split.RemoveAt(split.Count - 1);
            string path = string.Join(':', split);

            // Get symbol
            var template = await GetSymbolAsync(path, symbol);

            var lines = template.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).ToList();
            // Remove lines starting with # ( and add them later )
            while (lines[0].StartsWith("#"))
            {
                lines.RemoveAt(0);
            }

            // Prepare for string.Format()
            int i = 0;
            int format, startIndex, index;

            string start;
            char find;
            // "DEF value reference ..." to "DEF {0} {1} ..."
            start = "DEF "; format = 0; find = ' ';
            startIndex = start.Length;
            index = lines[i].IndexOf(find, startIndex);
            lines[i] = lines[i].Remove(startIndex, index - startIndex);
            lines[i] = lines[i].Insert(startIndex, "{" + format + "}");
            start = "DEF {0} "; format = 1;
            startIndex = start.Length;
            index = lines[i].IndexOf(find, startIndex);
            lines[i] = lines[i].Remove(startIndex, index - startIndex);
            lines[i] = lines[i].Insert(startIndex, "{" + format + "}");

            // Clear fields F0 - F3
            start = "Fx \"";
            find = '"';
            for (i = 1; i < 5; i++)
            {
                switch (i)
                {
                    case 1: format = 1; break;
                    case 2: format = 0; break;
                    case 3: format = 2; break;
                    case 4: format = 3; break;
                    default:
                        break;
                }
                startIndex = start.Length;
                index = lines[i].IndexOf(find, startIndex);
                lines[i] = lines[i].Remove(startIndex, index - startIndex);
                lines[i] = lines[i].Insert(startIndex, "{" + format + "}");
            }

            // Remove F4 and following
            while (lines[i].StartsWith("F"))
            {
                lines.RemoveAt(i);
            }

            // Insert {4} for custom fields
            lines.Insert(i, "{4}");

            // Find and remove ALIAS
            for (i = 0; i < lines.Count; i++)
            {
                if (!lines[i].StartsWith("ALIAS")) continue;

                lines.RemoveAt(i);
                break;
            }

            // Restore deleted #-Lines
            lines.Insert(0, "# {0}");
            lines.Insert(1, "#");

            return string.Join(Environment.NewLine, lines);
        }

        public async Task<IList<string>> GetSymbolsAsync(string library)
        {
            if (!File.Exists(library))
            {
                throw new FileNotFoundException($"File {library} not found!");
            }

            List<string> symbols = new List<string>();

            using (var file = File.OpenText(library))
            {
                while (file.Peek() > 0)
                {
                    var line = await file.ReadLineAsync();
                    if (!line.StartsWith("DEF")) continue;

                    int start, end;
                    start = "DEF ".Length;
                    end = line.IndexOf(' ', start);
                    symbols.Add(line.Substring(start, end - start));
                }
            }
            return symbols;
        }
    }
}