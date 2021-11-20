using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace KiCadDbLib.Services.KiCad.LibraryReader
{
    public class KiCadLibraryReaderMediator : ILibraryReader
    {
        private readonly ISettingsProvider _settingsProvider;

        public KiCadLibraryReaderMediator(ISettingsProvider settingsProvider)
        {
            _settingsProvider = settingsProvider;
        }

        public async Task<string[]> GetFootprintsAsync()
        {
            var reader = await GetReaderAsync()
                .ConfigureAwait(false);

            return await reader.GetFootprintsAsync()
                .ConfigureAwait(false);
        }

        public async Task<string[]> GetSymbolsAsync()
        {
            var reader = await GetReaderAsync()
                .ConfigureAwait(false);

            return await reader.GetSymbolsAsync()
                .ConfigureAwait(false);
        }

        private async Task<ILibraryReader> GetReaderAsync()
        {
            var isLegacy = await IsLegacyAsync()
                .ConfigureAwait(false);

            return isLegacy
                ? new LegacyKiCadLibraryReader(_settingsProvider)
                : new KiCad6LibraryReader(_settingsProvider);
        }

        private async Task<bool> IsLegacyAsync()
        {
            var settings = await _settingsProvider.GetSettingsAsync()
                .ConfigureAwait(false);

            return !Directory.EnumerateFiles(settings.SymbolsPath, $"*{FileExtensions.KicadSym}")
                .Any();
        }
    }
}