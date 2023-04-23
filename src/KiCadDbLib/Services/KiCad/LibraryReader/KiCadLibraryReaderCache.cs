using System;
using System.Linq;
using System.Threading.Tasks;

namespace KiCadDbLib.Services.KiCad.LibraryReader
{
    public class KiCadLibraryReaderCache : ILibraryReader
    {
        private readonly ISettingsProvider _settingsProvider;
        private readonly ILibraryReader _libraryReader;

        private string _footprintsPath = string.Empty;
        private string[] _footprints = Array.Empty<string>();

        private string _symbolsPath = string.Empty;
        private string[] _symbols = Array.Empty<string>();

        public KiCadLibraryReaderCache(ILibraryReader libraryReader, ISettingsProvider settingsProvider)
        {
            _libraryReader = libraryReader;
            _settingsProvider = settingsProvider;
        }

        public async Task<string[]> GetFootprintsAsync()
        {
            var settings = await _settingsProvider.GetWorkspaceSettings()
                   .ConfigureAwait(false);

            if (_footprintsPath != settings.FootprintsPath)
            {
                _footprintsPath = settings.FootprintsPath;
                _footprints = await _libraryReader.GetFootprintsAsync()
                    .ConfigureAwait(false);
            }

            return _footprints.ToArray();
        }

        public async Task<string[]> GetSymbolsAsync()
        {
            var settings = await _settingsProvider.GetWorkspaceSettings()
                .ConfigureAwait(false);

            if (_symbolsPath != settings.SymbolsPath)
            {
                _symbolsPath = settings.SymbolsPath;
                _symbols = await _libraryReader.GetSymbolsAsync()
                    .ConfigureAwait(false);
            }

            return _symbols.ToArray();
        }
    }
}