using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiCadDbLib.Services.KiCad.LibraryWriter
{
    public class KiCad6LibrarySymbolTemplateFactory
    {
        private readonly Dictionary<string, SNode> _symbolCache = new();
        private readonly Dictionary<string, SNode> _libraryCache = new();

        public async Task<SNode> GetSymbolTemplateAsync(string directory, LibraryItemInfo symbolInfo)
        {
            if (!_symbolCache.TryGetValue(symbolInfo.ToString(), out var symbol))
            {
                symbol = await ReadSymbol(directory, symbolInfo).ConfigureAwait(false);
                _symbolCache[symbolInfo.ToString()] = symbol;
            }

            return symbol.Clone();
        }

        private static void Clean(ref SNode symbol)
        {
            var footprintFiltersProperty = symbol.Childs
                .Where(child => child.Name == "property")
                .FirstOrDefault(property => property.Childs[0].Name == "ki_fp_filters");

            if (footprintFiltersProperty != null)
            {
                symbol.Remove(footprintFiltersProperty);
            }
        }

        private async Task<SNode> ReadSymbol(string directory, LibraryItemInfo symbolInfo)
        {
            SNode root = await ReadLibrary(directory, symbolInfo.Library)
                .ConfigureAwait(false);

            var symbol = root.Childs
                .Where(child => child.Name == "symbol")
                .SingleOrDefault(symbol => symbol.Childs[0].Name == symbolInfo.Name);

            if (symbol is null)
            {
                throw new KeyNotFoundException($"Symbol \"{symbolInfo}\" not found.");
            }

            Clean(ref symbol);
            return symbol;
        }

        private async Task<SNode> ReadLibrary(string directory, string libraryName)
        {
            var libraryFile = Path.Combine(directory, libraryName + FileExtensions.KicadSym);
            Debug.WriteLine($"{nameof(ReadLibrary)}: {libraryFile}");

            if (!_libraryCache.TryGetValue(libraryFile, out var library))
            {
                var input = await File.ReadAllTextAsync(libraryFile, Encoding.UTF8)
                    .ConfigureAwait(false);

                library = SNode.Parse(input);
                _libraryCache[libraryFile] = library;
            }

            return library;
        }
    }
}