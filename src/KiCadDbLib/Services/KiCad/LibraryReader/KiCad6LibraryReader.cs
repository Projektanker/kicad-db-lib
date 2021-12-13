using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KiCadDbLib.Services.KiCad.LibraryReader
{
    public class KiCad6LibraryReader : ILibraryReader
    {
        private readonly ISettingsProvider _settingsProvider;

        public KiCad6LibraryReader(ISettingsProvider settingsProvider)
        {
            _settingsProvider = settingsProvider;
        }

        public async Task<string[]> GetFootprintsAsync()
        {
            var settings = await _settingsProvider.GetSettingsAsync()
                .ConfigureAwait(false);

            var kicadFootprints = GetFootprintInfosFromDirectory(settings.FootprintsPath);

            return kicadFootprints
                .Select(x => x.ToString())
                .ToArray();
        }

        public async Task<string[]> GetSymbolsAsync()
        {
            var settings = await _settingsProvider.GetSettingsAsync()
                .ConfigureAwait(false);

            var kicadSymbols = await GetSymbolInfosFromDirectoryAsync(settings.SymbolsPath)
                .ConfigureAwait(false);

            return kicadSymbols
                 .ToArray();
        }

        private static IEnumerable<LibraryItemInfo> GetFootprintInfosFromDirectory(string directory)
        {
            if (!Directory.Exists(directory))
            {
                throw new DirectoryNotFoundException($"Directory \"{directory}\" not found.");
            }

            return Directory.EnumerateDirectories(directory, $"*{FileExtensions.Pretty}")
                .SelectMany(GetFootprintInfos);
        }

        private static LibraryItemInfo[] GetFootprintInfos(string footprintDirectory)
        {
            var library = new DirectoryInfo(footprintDirectory).Name[..^FileExtensions.Pretty.Length];

            return Directory.EnumerateFiles(footprintDirectory, $"*{FileExtensions.KicadMod}")
               .Select(Path.GetFileNameWithoutExtension)
               .Select(footprint => new LibraryItemInfo(library, footprint!))
               .ToArray();
        }

        private static async Task<IEnumerable<string>> GetSymbolInfosFromDirectoryAsync(string directory)
        {
            if (!Directory.Exists(directory))
            {
                throw new DirectoryNotFoundException($"Directory \"{directory}\" not found.");
            }

            return await Directory.EnumerateFiles(directory, $"*{FileExtensions.KicadSym}")
                .ToAsyncEnumerable()
                .SelectMany(GetSymbolInfosAsync)
                .ToArrayAsync()
                .ConfigureAwait(false);
        }

        private static async IAsyncEnumerable<string> GetSymbolInfosAsync(string libraryFile)
        {
            var sw = Stopwatch.StartNew();
            var regex = new Regex("\\(symbol \"((?:.+?):(?:.+?))\" (?!.*?extends)");
            var lines = await File.ReadAllTextAsync(libraryFile, Encoding.UTF8)
                .ConfigureAwait(false);

            var symbols = regex.Matches(lines)
                .Select(match => match.Groups[1].Value);

            foreach (var symbol in symbols)
            {
                yield return symbol;
            }

            sw.Stop();
            Debug.WriteLine($"Get Symbols from {libraryFile}: {sw.ElapsedMilliseconds}ms");
        }
    }
}