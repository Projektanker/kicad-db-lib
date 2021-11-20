using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KiCadDbLib.Services.KiCad
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
                 .Select(x => x.ToString())
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

        private static async Task<IEnumerable<LibraryItemInfo>> GetSymbolInfosFromDirectoryAsync(string directory)
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

        private static async IAsyncEnumerable<LibraryItemInfo> GetSymbolInfosAsync(string libraryFile)
        {
            var symbolRegex = new Regex("\\(symbol \"(.+?):(.+?)\"");

            var lines = await File.ReadAllLinesAsync(libraryFile, Encoding.UTF8)
                .ConfigureAwait(false);

            var symbols = lines
                .Select(line => symbolRegex.Match(line))
                .Where(match => match.Success)
                .Select(match => new LibraryItemInfo(match.Groups[1].Value, match.Groups[2].Value));

            foreach (var symbol in symbols)
            {
                yield return symbol;
            }
        }
    }
}