using System.Threading.Tasks;

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

            return new KiCadLibraryWriter(settings.SymbolsPath, outputDirectory, libraryName);
        }
    }
}