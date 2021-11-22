using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiCadDbLib.Services.KiCad.LibraryWriter
{
    internal class KiCad6LibrarySymbolTemplateFactory
    {
        private readonly Dictionary<string, SNode> _symbolCache = new();
        private string _cachedLibraryName = string.Empty;
        private SNode _cachedLibrary = new();

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
                .FirstOrDefault(property => property.Childs[0].Name?.Trim('"') == "ki_fp_filters");

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
                .SingleOrDefault(symbol => symbol.Childs[0].Name?.Trim('"') == symbolInfo.ToString());

            if (symbol is null)
            {
                throw new KeyNotFoundException($"Symbol \"{symbolInfo}\" not found.");
            }

            Clean(ref symbol);

            return symbol;
        }

        private async Task<SNode> ReadLibrary(string directory, string libraryName)
        {
            if (_cachedLibraryName != libraryName)
            {
                var libraryFile = Path.Combine(directory, libraryName + FileExtensions.KicadSym);
                var input = await File.ReadAllTextAsync(libraryFile, Encoding.UTF8)
                    .ConfigureAwait(false);

                _cachedLibrary = SNode.Parse(input);
                _cachedLibraryName = libraryName;
            }

            return _cachedLibrary;
        }
    }
}