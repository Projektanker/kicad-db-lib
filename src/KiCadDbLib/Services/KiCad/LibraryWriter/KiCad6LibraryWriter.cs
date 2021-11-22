using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KiCadDbLib.Models;

namespace KiCadDbLib.Services.KiCad.LibraryWriter
{
    internal sealed class KiCad6LibraryWriter : ILibraryWriter
    {
        private const string _property = "property";
        private readonly KiCad6LibrarySymbolTemplateFactory _libraryReader;
        private readonly string _symbolsDirectory;
        private readonly string _outputDirectory;
        private readonly string _libraryName;
        private readonly SNode _root;

        public KiCad6LibraryWriter(string symbolsDirectory, string outputDirectory, string libraryName)
        {
            _symbolsDirectory = symbolsDirectory;
            _outputDirectory = outputDirectory;
            _libraryName = libraryName;

            _libraryReader = new KiCad6LibrarySymbolTemplateFactory();
            _root = new SNode();
        }

        public void Dispose()
        {
            // nothing to dispose
        }

        public ValueTask DisposeAsync()
        {
            return ValueTask.CompletedTask;
        }

        public Task WriteStartLibrary()
        {
            _root.Name = "kicad_symbol_lib";
            _root.Add(new SNode("version", new SNode("20201005")));
            _root.Add(new SNode("generator", new SNode("kicad_db_lib")));
            return Task.CompletedTask;
        }

        public async Task WriteEndLibrary()
        {
            var outputPath = Path.Combine(_outputDirectory, _libraryName + FileExtensions.KicadSym);
            var content = _root.ToString();
            var uft8WithoutBOM = new UTF8Encoding(false);
            await File.WriteAllTextAsync(outputPath, content, uft8WithoutBOM)
                .ConfigureAwait(false);
        }

        public async Task WritePartAsync(Part part)
        {
            var symbolInfo = LibraryItemInfo.Parse(part.Symbol);
            var template = await _libraryReader.GetSymbolTemplateAsync(_symbolsDirectory, symbolInfo)
                .ConfigureAwait(false);

            template.Childs[0].Name = $"\"{_libraryName}:{part.Value}\"";
            UpdateUnitIds(template, symbolInfo.Name, part.Value);

            SetPropertyByName(template, "Reference", part.Reference);
            SetPropertyByName(template, "Value", part.Value);
            SetPropertyByName(template, "Footprint", part.Footprint);
            SetPropertyByName(template, "Datasheet", string.IsNullOrEmpty(part.Datasheet) ? "~" : part.Datasheet);
            SetPropertyByName(template, "ki_keywords", part.Keywords);
            SetPropertyByName(template, "ki_description", part.Description);

            var customFields = part.CustomFields.OrderBy(cf => cf.Key).ToArray();
            for (int i = 0; i < customFields.Length; i++)
            {
                AddCustomProperty(template, i, customFields[i]);
            }

            _root.Add(template);
        }

        private static void UpdateUnitIds(SNode symbol, string symbolName, string partValue)
        {
            var units = symbol.Childs
                .Where(child => child.Name == "symbol");
            foreach (var unit in units)
            {
                unit.Childs[0].Name = unit.Childs[0].Name?.Replace(symbolName, partValue, StringComparison.Ordinal);
            }
        }

        private static void SetPropertyByName(SNode symbol, string propertyName, string value)
        {
            var property = symbol.Childs
                .Where(child => child.Name == _property)
                .SingleOrDefault(property => property.Childs[0].Name!.Trim('"') == propertyName);

            if (property is null)
            {
                var symbolName = symbol.Childs[0].Name;
                throw new KeyNotFoundException($"Property \"{propertyName}\" not found in symbol\"{symbolName}\".");
            }

            property.Childs[1].Name = $"\"{value}\"";
        }

        private static void AddCustomProperty(SNode symbol, int index, KeyValuePair<string, string> property)
        {
            var customProperty = CreateCustomProperty(index + 6, property);
            var nodeIndex = IndexOfLastProperty(symbol) + 1;
            symbol.Insert(nodeIndex, customProperty);
        }

        private static SNode CreateCustomProperty(int propertyId, KeyValuePair<string, string> property)
        {
            if (propertyId < 6)
            {
                throw new ArgumentOutOfRangeException(nameof(propertyId), "Must be greater or equal 6.");
            }

            var id = new SNode("id", new SNode(propertyId.ToString()));
            var at = new SNode("at", new SNode("0"), new SNode("0"), new SNode("0"));

            var font = new SNode("font", new SNode("size", new SNode("1.27"), new SNode("1.27")));
            var effects = new SNode("effects", font, new SNode("hide"));

            return new SNode(_property, new SNode($"\"{property.Key}\""), new SNode($"\"{property.Value}\""), id, at, effects);
        }

        private static int IndexOfLastProperty(SNode symbol)
        {
            return symbol.Childs
                .Select((child, index) => (Child: child, Index: index))
                .Last(x => x.Child.Name == _property)
                .Index;
        }
    }
}