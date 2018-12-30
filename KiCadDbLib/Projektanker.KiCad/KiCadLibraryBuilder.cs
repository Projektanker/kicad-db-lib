using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projektanker.KiCad
{
    public class KiCadLibraryBuilder
    {
        private readonly KiCadLibraryReader _reader;

        public KiCadLibraryBuilder()
        {
            _reader = new KiCadLibraryReader();
        }

        public async Task BuildAsync(IList<KiCadPart> kiCadParts, string outputDirectory, bool clearOutputDirectory = false)
        {
            if (clearOutputDirectory)
            {
                var files = Directory.EnumerateFiles(outputDirectory)
                    .Where(file => file.ToLower().EndsWith(".lib") || file.ToLower().EndsWith(".dcm"));
                foreach (var file in files)
                {
                    File.Delete(file);
                }
            }

            // Sort parts by Library and Value
            IOrderedEnumerable<KiCadPart> parts = kiCadParts.OrderBy(p => p.Library).ThenBy(p => p.Value);

            string library = null;
            StreamWriter lib = null, dcm = null;
            try
            {
                foreach (var part in parts)
                {
                    if (part.Library != library)
                    {
                        // Close previous library files
                        if (lib != null)
                        {
                            await lib.WriteLineAsync("#End Library");
                            lib.Close();
                        }

                        if (dcm != null)
                        {
                            await dcm.WriteLineAsync("#End Doc Library");
                            dcm.Close();
                        }

                        // Create new library files
                        library = part.Library;
                        lib = File.CreateText(Path.Combine(outputDirectory, $"{library}.lib"));
                        dcm = File.CreateText(Path.Combine(outputDirectory, $"{library}.dcm"));

                        // Write file head
                        await lib.WriteLineAsync("EESchema-LIBRARY Version 2.4");
                        await lib.WriteLineAsync("#encoding utf-8");
                        await lib.WriteLineAsync("#");

                        await dcm.WriteLineAsync("EESchema-DOCLIB  Version 2.0");
                        await dcm.WriteLineAsync("#");
                    }

                    var template = await _reader.GetSymbolTemplateAsync(part);

                    // Fill part and remove empty lines
                    var partString =
                        string.Format(template,
                        part.Value,
                        part.Reference,
                        part.Footprint,
                        part.Datasheet,
                        CreateCustomFields(part.CustomFields))
                        .Replace(Environment.NewLine + Environment.NewLine, Environment.NewLine);

                    // Write .lib file
                    await lib.WriteLineAsync(partString);

                    // Write .dcm file
                    if (!string.IsNullOrWhiteSpace(part.Value))
                        await dcm.WriteLineAsync($"$CMP {part.Value}");
                    await dcm.WriteLineAsync($"D {part.Description}");
                    if (!string.IsNullOrWhiteSpace(part.Description))
                        await dcm.WriteLineAsync($"K {part.Keywords}");
                    if (!string.IsNullOrWhiteSpace(part.Datasheet))
                        await dcm.WriteLineAsync($"F {part.Datasheet}");

                    await dcm.WriteLineAsync($"$ENDCMP");
                    await dcm.WriteLineAsync($"#");
                }
            }
            finally
            {
                if (lib != null)
                {
                    lib.Close();
                }

                if (dcm != null)
                {
                    dcm.Close();
                }
            }
        }

        private static string CreateCustomField(int fieldNumber, string key, string value)
        {
            if (fieldNumber < 4)
            {
                throw new ArgumentException("Fieldnumber must be greater or equal 4!");
            }

            return $"F{fieldNumber} \"{value}\" {100 * (fieldNumber - 3)} {100 * (fieldNumber - 2)} 50 H I C CNN \"{key}\"";
        }

        private static string CreateCustomFields(Dictionary<string, string> fields)
        {
            if (fields == null)
                return null;

            StringBuilder text = new StringBuilder();
            int i = 4;
            foreach (var field in fields)
            {
                text.AppendLine(CreateCustomField(i, field.Key, field.Value));
                i++;
            }
            return text.ToString();
        }
    }
}