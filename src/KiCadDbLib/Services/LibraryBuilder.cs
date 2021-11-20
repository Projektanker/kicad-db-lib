using System.Linq;
using System.Threading.Tasks;
using KiCadDbLib.Services.KiCad;

namespace KiCadDbLib.Services
{
    public class LibraryBuilder : ILibraryBuilder
    {
        private readonly ISettingsProvider _settingsProvider;
        private readonly IPartRepository _partRepository;
        private readonly ILibraryWriterFactory _libraryWriterFactory;

        public LibraryBuilder(ISettingsProvider settingsProvider, IPartRepository partRepository, ILibraryWriterFactory libraryWriterFactory)
        {
            _settingsProvider = settingsProvider;
            _partRepository = partRepository;
            _libraryWriterFactory = libraryWriterFactory;
        }

        public async Task Build()
        {
            var settings = await _settingsProvider.GetSettingsAsync().ConfigureAwait(false);
            var parts = await _partRepository.GetPartsAsync().ConfigureAwait(false);

            await KiCadLibraryWriter.ClearDirectoryAsync(settings.OutputPath).ConfigureAwait(false);

            var groupedParts = parts
                 .GroupBy(part => part.Library);

            foreach (var group in groupedParts)
            {
                await using var writer = await _libraryWriterFactory.CreateWriterAsync(settings.OutputPath, group.Key!)
                    .ConfigureAwait(false);

                await writer.WriteStartLibrary().ConfigureAwait(false);

                foreach (var part in group.OrderBy(part => part.Value))
                {
                    part.CustomFields = part.CustomFields
                        .Join(settings.CustomFields, cf => cf.Key, key => key, (cf, _) => cf)
                        .ToDictionary(cf => cf.Key, cf => cf.Value);

                    await writer.WritePartAsync(part).ConfigureAwait(false);
                }

                await writer.WriteEndLibrary().ConfigureAwait(false);
            }
        }
    }
}