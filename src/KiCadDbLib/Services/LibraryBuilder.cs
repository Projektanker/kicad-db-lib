using System.Linq;
using System.Threading.Tasks;
using KiCadDbLib.Models;
using KiCadDbLib.Services.KiCad;

namespace KiCadDbLib.Services
{
    public class LibraryBuilder : ILibraryBuilder
    {
        private readonly ISettingsProvider _settingsProvider;
        private readonly IPartRepository _partRepository;

        public LibraryBuilder(ISettingsProvider settingsProvider, IPartRepository partRepository)
        {
            _settingsProvider = settingsProvider;
            _partRepository = partRepository;
        }

        public async Task Build()
        {
            var settings = await _settingsProvider.GetSettingsAsync().ConfigureAwait(false);
            var parts = await _partRepository.GetPartsAsync().ConfigureAwait(false);

            await KiCadLibraryBuilder.ClearDirectoryAsync(settings.OutputPath).ConfigureAwait(false);

            var groupedParts = parts
                 .GroupBy(part => part.Library);

            foreach (IGrouping<string, Part> group in groupedParts)
            {
                using var builder = new KiCadLibraryBuilder(settings.SymbolsPath, settings.OutputPath, group.Key);
                await builder.WriteStartLibrary().ConfigureAwait(false);

                foreach (var part in group.OrderBy(part => part.Value))
                {
                    var symbolInfo = LibraryItemInfo.Parse(part.Symbol);

                    await builder.WritePartAsync(
                        reference: part.Reference,
                        value: part.Value,
                        symbol: symbolInfo,
                        footprint: part.Footprint,
                        description: part.Description,
                        keywords: part.Keywords,
                        datasheet: part.Datasheet,
                        customFields: part.CustomFields.Where(cf => settings.CustomFields.Contains(cf.Key))).ConfigureAwait(false);
                }

                await builder.WriteEndLibrary().ConfigureAwait(false);
            }
        }
    }
}