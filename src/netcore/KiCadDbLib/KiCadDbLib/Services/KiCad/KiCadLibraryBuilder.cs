using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KiCadDbLib.Services.KiCad
{
    internal sealed class KiCadLibraryBuilder : IDisposable, IAsyncDisposable
    {
        private readonly TextWriter _dcmWriter;
        private readonly string _libraryName;
        private readonly KiCadLibraryReader _libraryReader;
        private readonly TextWriter _libWriter;
        private readonly string _outputDirectory;

        public KiCadLibraryBuilder(string outputDirectory, string libraryName)
        {
            _outputDirectory = outputDirectory;
            _libraryName = libraryName;

            _libWriter = new StreamWriter(
                path: Path.Combine(outputDirectory, libraryName + FileExtensions.Lib),
                append: false,
                encoding: Encoding.UTF8);
            _dcmWriter = new StreamWriter(
                path: Path.Combine(outputDirectory, libraryName + FileExtensions.Dcm),
                append: false,
                encoding: Encoding.UTF8);

            _libraryReader = new KiCadLibraryReader();
        }

        public static async Task ClearDirectoryAsync(string directory)
        {
            IEnumerable<string> files = Directory.EnumerateFiles(directory, $"*{FileExtensions.Lib}")
                .Concat(Directory.EnumerateFiles(directory, $"*{FileExtensions.Dcm}"));

            foreach (string file in files)
            {
                await Task.Run(() => File.Delete(file));
            }
        }

        public void Dispose()
        {
            _libWriter.Dispose();
            _dcmWriter.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            await _libWriter.DisposeAsync();
            await _dcmWriter.DisposeAsync();
        }

        public async Task WriteEndLibrary()
        {
            await Task.WhenAll(
                WriteEndDcmAsync(),
                WriteEndLibAsync());
        }

        public async Task WritePartAsync(
            string reference,
            string value,
            LibraryItemInfo symbol,
            string footprint,
            string description,
            string keywords,
            string datasheet,
            IEnumerable<KeyValuePair<string, string>> customFields)
        {
            var dcm = WritePartToDcmAsync(
                value: value,
                description: description,
                keywords: keywords,
                datasheet: datasheet);

            var lib = WritePartToLibAsync(
                reference: reference,
                value: value,
                symbol: symbol,
                footprint: footprint,
                datasheet: datasheet,
                customFields: customFields);

            await Task.WhenAll(dcm, lib);
        }

        public async Task WriteStartLibrary()
        {
            await Task.WhenAll(
                WriteStartDcmAsync(),
                WriteStartLibAsync());
        }

        private string CreateCustomField(int fieldNumber, string key, string value)
        {
            if (fieldNumber < 4)
            {
                throw new ArgumentOutOfRangeException(nameof(fieldNumber), "Must be greater or equal 4.");
            }

            return $"F{fieldNumber} \"{value ?? "-"}\" " +
                $"{100 * (fieldNumber - 3)} {100 * (fieldNumber - 2)} " +
                $"50 H I C CNN \"{key}\"";
        }

        private IEnumerable<string> CreateCustomFields(IEnumerable<KeyValuePair<string, string>> fields)
        {
            if (fields is null)
            {
                return Enumerable.Empty<string>();
            }

            var startIndex = 4;
            return fields
                .Select((field, index) => CreateCustomField(index + startIndex, field.Key, field.Value));
        }

        private async Task<IEnumerable<string>> CreateSymbolAsync(
            LibraryItemInfo symbolInfo,
            string reference,
            string value,
            string footprint,
            string datasheet,
            IEnumerable<KeyValuePair<string, string>> customFields,
            bool bufferLibrary = true)
        {
            // Get symbol
            string[] template = await _libraryReader.GetSymbolAsync(symbolInfo, bufferLibrary);

            // Remove empty lines
            var filtered = template.Where(line => !string.IsNullOrWhiteSpace(line));

            // Remove lines starting with '#'
            filtered = filtered.Where(line => !line.StartsWith('#'));

            // Remove ALIAS
            filtered = filtered.Where(line => !line.StartsWith("ALIAS", StringComparison.OrdinalIgnoreCase));

            // Remove FPLIST
            static bool isPartOfFPList(string line, ref bool insideFPList)
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
            filtered = filtered.Where(line => !isPartOfFPList(line, ref insideFPList));

            // Done filtering.
            var result = filtered.ToList();

            // DEF value reference ...
            // F0 "Reference" ...
            // F1 "Value" ...
            // F2 "Footprint" ...
            // F3 "Datasheet" ...
            // F3+n "custom field value" ... "custom field name"

            // DEF value reference ...
            result[0] = Regex.Replace(result[0], "^DEF \\S+ \\S ", $"DEF {value} {reference} ");

            // F0 - F3
            int index;
            string[] replacement = new[] { reference, value, footprint, datasheet };
            for (index = 1; index < 5; index++)
            {
                var fieldIndex = index - 1;
                result[index] = Regex.Replace(
                    result[index],
                    $"^F{fieldIndex} \".*?\"",
                    $"F{fieldIndex} \"{replacement[fieldIndex]}\"");
            }

            // Remove remaining F...
            while (result[index].StartsWith('F'))
            {
                result.RemoveAt(index);
            }

            // Add custom fields
            result.InsertRange(index, CreateCustomFields(customFields));

            return result;
        }
        private async Task WriteEndDcmAsync()
        {
            await _dcmWriter.WriteLineAsync("#End Doc Library");
        }

        private async Task WriteEndLibAsync()
        {
            await _libWriter.WriteLineAsync("#End Library");
        }

        private async Task WritePartToDcmAsync(
            string value,
            string description,
            string keywords,
            string datasheet)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                await _dcmWriter.WriteLineAsync($"$CMP {value}");
            }

            if (!string.IsNullOrWhiteSpace(description))
            {
                await _dcmWriter.WriteLineAsync($"D { description}");
            }

            if (!string.IsNullOrWhiteSpace(keywords))
            {
                await _dcmWriter.WriteLineAsync($"K { keywords}");
            }

            if (!string.IsNullOrWhiteSpace(datasheet))
            {
                await _dcmWriter.WriteLineAsync($"F { datasheet}");
            }

            await _dcmWriter.WriteLineAsync("$ENDCMP");
            await _dcmWriter.WriteLineAsync('#');
        }

        private async Task WritePartToLibAsync(
            string reference,
            string value,
            LibraryItemInfo symbol,
            string footprint,
            string datasheet,
            IEnumerable<KeyValuePair<string, string>> customFields)
        {
            await _libWriter.WriteLineAsync($"# {value}");
            await _libWriter.WriteLineAsync('#');
            var createdSymbol = await CreateSymbolAsync(symbol, reference, value, footprint, datasheet, customFields);
            foreach (var line in createdSymbol)
            {
                await _libWriter.WriteLineAsync(line);
            }

            await _libWriter.WriteLineAsync('#');
        }

        private async Task WriteStartDcmAsync()
        {
            await _dcmWriter.WriteLineAsync("EESchema-DOCLIB  Version 2.0");
            await _dcmWriter.WriteLineAsync("*");
        }

        private async Task WriteStartLibAsync()
        {
            await _libWriter.WriteLineAsync("EESchema-LIBRARY Version 2.4");
            await _libWriter.WriteLineAsync("#encoding utf-8");
            await _libWriter.WriteLineAsync("#");
        }
    }
}