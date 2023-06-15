using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        public KiCad6LibraryWriter(KiCad6LibrarySymbolTemplateFactory libraryReader, string symbolsDirectory, string outputDirectory, string libraryName)
        {
            _symbolsDirectory = symbolsDirectory;
            _outputDirectory = outputDirectory;
            _libraryName = libraryName;

            _libraryReader = libraryReader;
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

        public async Task WriteStartLibrary()
        {
            var symbolLibFile = Directory
                .EnumerateFiles(_symbolsDirectory, $"*{FileExtensions.KicadSym}")
                .FirstOrDefault();

            var version = default(string);
            if (symbolLibFile is not null)
            {
                var versionRegex = new Regex(@"(?<=\(version )\d+(?=\))");
                var symbolLibLines = File.ReadLinesAsync(symbolLibFile);
                version = await symbolLibLines
                    .Select(line => versionRegex.Match(line))
                    .Where(match => match.Success)
                    .Select(match => match.Value)
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false);
            }

            _root.Name = "kicad_symbol_lib";
            _root.Add(new SNode("version", new SNode(version ?? "20201005")));
            _root.Add(new SNode("generator", new SNode("kicad_db_lib")));
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
            var symbol = await _libraryReader.GetSymbolTemplateAsync(_symbolsDirectory, symbolInfo)
                .ConfigureAwait(false);

            SetSymbolId(symbol, part.Value);
            UpdateUnitIds(symbol, symbolInfo.Name, part.Value);

            SetOrAddPropertyByName(symbol, "Reference", part.Reference);
            SetOrAddPropertyByName(symbol, "Value", part.Value);
            SetOrAddPropertyByName(symbol, "Footprint", part.Footprint);
            SetOrAddPropertyByName(symbol, "Datasheet", string.IsNullOrEmpty(part.Datasheet) ? "~" : part.Datasheet);
            SetOrAddPropertyByName(symbol, "ki_keywords", part.Keywords);
            SetOrAddPropertyByName(symbol, "ki_description", part.Description);

            foreach (var customField in part.CustomFields.OrderBy(cf => cf.Key))
            {
                AddCustomProperty(symbol, customField);
            }

            _root.Add(symbol);
        }

        private static void SetSymbolId(SNode symbol, string partValue)
        {
            var symbolId = symbol.Childs[0];
            symbolId.Name = partValue;
            symbolId.IsString = true;
        }

        private static void UpdateUnitIds(SNode symbol, string symbolName, string partValue)
        {
            var unitIds = symbol.Childs
                .Where(child => child.Name == "symbol")
                .Select(unit => unit.Childs[0]);
            foreach (var unitId in unitIds)
            {
                unitId.Name = unitId.Name?.Replace(symbolName, partValue, StringComparison.Ordinal);
            }
        }

        private static void SetOrAddPropertyByName(SNode symbol, string propertyName, string value)
        {
            var property = symbol.Childs
                .Where(child => child.Name == _property)
                .SingleOrDefault(property => property.Childs[0].Name == propertyName);

            if (property is not null)
            {
                property.Childs[1].Name = value;
                property.Childs[1].IsString = true;
            }
            else
            {
                var index = GetIndexOfLastProperty(symbol) + 1;
                property = CreateCustomProperty(null, KeyValuePair.Create(propertyName, value));
                symbol.Insert(index, property);
            }
        }

        private static void AddCustomProperty(SNode symbol, KeyValuePair<string, string> property)
        {
            var id = GetIdOfLastProperty(symbol) + 1;
            var customProperty = CreateCustomProperty(id, property);
            var index = GetIndexOfLastProperty(symbol) + 1;
            symbol.Insert(index, customProperty);
        }

        private static SNode CreateCustomProperty(int? propertyId, KeyValuePair<string, string> property)
        {
            var childs = new List<SNode>()
            {
                new SNode(property.Key, isString: true),
                new SNode(property.Value, isString: true)
            };

            if (propertyId.HasValue)
            {
                childs.Add(new SNode("id", new SNode(propertyId.Value.ToString())));
            }

            childs.AddRange(new[]
            {
                new SNode("at", new SNode("0"), new SNode("0"), new SNode("0")),
                new SNode(
                    "effects",
                    new SNode("font", new SNode("size", new SNode("1.27"), new SNode("1.27"))),
                    new SNode("hide")),
            });

            return new SNode(_property, childs);
        }

        private static int? GetIdOfLastProperty(SNode symbol)
        {
            static int? GetIdOfProperty(SNode property)
            {
                var idNode = property.Childs.FirstOrDefault(child => child.Name == "id");
                if (idNode is null)
                {
                    return null;
                }

                return int.Parse(idNode.Childs[0].Name!);
            }

            return symbol.Childs
                .Where(child => child.Name == _property)
                .Select(GetIdOfProperty)
                .Last();
        }

        private static int GetIndexOfLastProperty(SNode symbol)
        {
            return symbol.Childs
                .Select((child, index) => (Child: child, Index: index))
                .Last(x => x.Child.Name == _property)
                .Index;
        }
    }
}