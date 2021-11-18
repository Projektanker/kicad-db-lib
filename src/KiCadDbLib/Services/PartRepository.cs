using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KiCadDbLib.Models;
using Newtonsoft.Json;

namespace KiCadDbLib.Services
{
    public class PartRepository : IPartRepository
    {
        private readonly ISettingsProvider _settingsProvider;

        public PartRepository(ISettingsProvider settingsProvider)
        {
            _settingsProvider = settingsProvider;
        }

        public async Task AddOrUpdateAsync(Part part)
        {
            var settings = await _settingsProvider.GetSettingsAsync()
                .ConfigureAwait(false);

            if (string.IsNullOrEmpty(part.Id))
            {
                part.Id = await GetNewId(settings).ConfigureAwait(false);
            }

            var filePath = await GetFilePathAsync(part.Id, settings).ConfigureAwait(false);
            var json = JsonConvert.SerializeObject(part);
            await File.WriteAllTextAsync(filePath, json).ConfigureAwait(false);
        }

        public async Task DeleteAsync(string id)
        {
            var filePath = await GetFilePathAsync(id).ConfigureAwait(false);
            File.Delete(filePath);
        }

        public async Task<string[]> GetLibrariesAsync()
        {
            return (await GetPartsAsync().ConfigureAwait(false))
                .Select(part => part.Library)
                .Distinct()
                .OrderBy(lib => lib)
                .ToArray();
        }

        public async Task<Part> GetPartAsync(string id)
        {
            var settings = await _settingsProvider.GetSettingsAsync().ConfigureAwait(false);
            var filePath = await GetFilePathAsync(id, settings).ConfigureAwait(false);
            return await GetPartAsync(filePath, settings).ConfigureAwait(false);
        }

        public async Task<Part[]> GetPartsAsync()
        {
            var settings = await _settingsProvider.GetSettingsAsync().ConfigureAwait(false);

            IEnumerable<Task<Part>> tasks = Directory.EnumerateFiles(settings.DatabasePath)
                .Select(file => GetPartAsync(file, settings));

            Part[] parts = await Task.WhenAll(tasks).ConfigureAwait(false);
            return parts;
        }

        private static async Task<Part> GetPartAsync(string file, Settings settings)
        {
            var json = await File.ReadAllTextAsync(file).ConfigureAwait(false);
            var part = JsonConvert.DeserializeObject<Part>(json);
            part!.CustomFields = part.CustomFields
                .Join(settings.CustomFields, cf => cf.Key, customFieldKey => customFieldKey, (cf, _) => cf)
                .ToDictionary(kv => kv.Key, kv => kv.Value);

            return part;
        }

        private async Task<string> GetNewId(Settings? settings = null)
        {
            settings ??= await _settingsProvider.GetSettingsAsync().ConfigureAwait(false);
            var newId = Directory.EnumerateFiles(settings.DatabasePath)
                .Select(Path.GetFileNameWithoutExtension)
                .Select(id => int.TryParse(id, out var result) ? result : default)
                .DefaultIfEmpty()
                .Max() + 1;

            return newId.ToString(CultureInfo.InvariantCulture);
        }

        private async Task<string> GetFilePathAsync(string id, Settings? settings = null)
        {
            settings ??= await _settingsProvider.GetSettingsAsync().ConfigureAwait(false);
            return Path.Combine(settings.DatabasePath, $"{id}.json");
        }
    }
}