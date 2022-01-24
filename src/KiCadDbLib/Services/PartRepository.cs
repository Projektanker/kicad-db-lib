using System;
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
        private readonly Lazy<Task<Settings>> _settings;

        public PartRepository(ISettingsProvider settingsProvider)
        {
            _settings = new(settingsProvider.GetSettingsAsync);
        }

        public async Task AddOrUpdateAsync(Part part)
        {
            if (string.IsNullOrEmpty(part.Id))
            {
                part.Id = await GetNewId().ConfigureAwait(false);
            }

            var filePath = await GetFilePathAsync(part.Id)
                .ConfigureAwait(false);

            var json = JsonConvert.SerializeObject(part);
            await File.WriteAllTextAsync(filePath, json)
                .ConfigureAwait(false);
        }

        public async Task DeleteAsync(string id)
        {
            var filePath = await GetFilePathAsync(id)
                .ConfigureAwait(false);

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
            var filePath = await GetFilePathAsync(id).ConfigureAwait(false);
            var part = await Deserialize(filePath).ConfigureAwait(false);
            return part!;
        }

        public async Task<Part[]> GetPartsAsync()
        {
            var directory = await TryGetDatabasePath()
                .ConfigureAwait(false);

            if (directory is null)
            {
                return Array.Empty<Part>();
            }

            return await Directory.EnumerateFiles(directory)
                .ToAsyncEnumerable()
                .SelectAwait(Deserialize)
                .ToArrayAsync()
                .ConfigureAwait(false);
        }

        private static async ValueTask<Part> Deserialize(string filePath)
        {
            var json = await File.ReadAllTextAsync(filePath).ConfigureAwait(false);
            var part = JsonConvert.DeserializeObject<Part>(json);
            return part!;
        }

        private async Task<string> GetNewId()
        {
            var directory = await GetDatabasePath()
                .ConfigureAwait(false);

            var newId = Directory.EnumerateFiles(directory)
                .Select(Path.GetFileNameWithoutExtension)
                .Select(id => int.TryParse(id, out var result) ? result : default)
                .DefaultIfEmpty()
                .Max() + 1;

            return newId.ToString(CultureInfo.InvariantCulture);
        }

        private async Task<string> GetFilePathAsync(string id)
        {
            var directory = await GetDatabasePath()
                .ConfigureAwait(false);

            return Path.Combine(directory, $"{id}.json");
        }

        private async Task<string?> TryGetDatabasePath()
        {
            var settings = await _settings.Value.ConfigureAwait(false);
            return !string.IsNullOrEmpty(settings.DatabasePath) && Directory.Exists(settings.DatabasePath)
                ? settings.DatabasePath
                : null;
        }

        private async Task<string> GetDatabasePath()
        {
            var settings = await _settings.Value.ConfigureAwait(false);
            if (string.IsNullOrEmpty(settings.DatabasePath))
            {
                throw new DirectoryNotFoundException("The database directory is not set.");
            }
            else if (!Directory.Exists(settings.DatabasePath))
            {
                throw new DirectoryNotFoundException($"The database directory \"{settings.DatabasePath}\" does not exists");
            }
            else
            {
                return settings.DatabasePath;
            }
        }
    }
}