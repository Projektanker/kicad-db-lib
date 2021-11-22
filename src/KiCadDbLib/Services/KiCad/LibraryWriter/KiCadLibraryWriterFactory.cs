using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KiCadDbLib.Models;

namespace KiCadDbLib.Services.KiCad.LibraryWriter
{
    public class KiCadLibraryWriterFactory : ILibraryWriterFactory
    {
        private readonly ISettingsProvider _settingsProvider;

        public KiCadLibraryWriterFactory(ISettingsProvider settingsProvider)
        {
            _settingsProvider = settingsProvider;
        }

        public async Task<ILibraryWriter> CreateWriterAsync(string outputDirectory, string libraryName)
        {
            var settings = await _settingsProvider.GetSettingsAsync()
                .ConfigureAwait(false);

            var isLegacy = IsLegacy(settings);

            return isLegacy
                ? new LegacyKiCadLibraryWriter(settings.SymbolsPath, outputDirectory, libraryName)
                : new KiCad6LibraryWriter(settings.SymbolsPath, outputDirectory, libraryName);
        }

        private static bool IsLegacy(Settings settings)
        {
            return !Directory.EnumerateFiles(settings.SymbolsPath, $"*{FileExtensions.KicadSym}")
                .Any();
        }
    }
}