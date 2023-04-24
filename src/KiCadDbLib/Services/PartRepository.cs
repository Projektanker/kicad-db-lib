using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using KiCadDbLib.Models;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace KiCadDbLib.Services
{
    public class PartRepository : IPartRepository
    {
        private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            AllowTrailingCommas = true,
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
        };

        private readonly ISettingsProvider _settingsProvider;

        public PartRepository(ISettingsProvider settingsProvider)
        {
            _settingsProvider = settingsProvider;
        }

        public async Task AddOrUpdateAsync(Part part)
        {
            if (part.Id == 0)
            {
                part.Id = await GetNewId().ConfigureAwait(false);
            }

            var filePath = await GetFilePathAsync(part.Id)
                .ConfigureAwait(false);
            await Serialize(part, filePath).ConfigureAwait(false);
        }

        public async Task DeleteAsync(int id)
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

        public async Task<Part> GetPartAsync(int id)
        {
            var filePath = await GetFilePathAsync(id).ConfigureAwait(false);
            var part = await Deserialize(filePath).ConfigureAwait(false);
            return part;
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

        private static async Task Serialize(Part part, string filePath)
        {
            var entity = PartEntity.From(part);
            var json = JsonSerializer.Serialize(entity, _jsonSerializerOptions);
            await File.WriteAllTextAsync(filePath, json)
                .ConfigureAwait(false);
        }

        private static async ValueTask<Part> Deserialize(string filePath)
        {
            var json = await File.ReadAllTextAsync(filePath).ConfigureAwait(false);
            var entity = JsonSerializer.Deserialize<PartEntity>(json, _jsonSerializerOptions)!;
            var id = GetId(filePath);
            return entity.ToPart(id);
        }

        private static int GetId(string filePath)
        {
            return int.Parse(
                Path.GetFileNameWithoutExtension(filePath),
                CultureInfo.InvariantCulture);
        }

        private async Task<int> GetNewId()
        {
            var directory = await GetDatabasePath()
                .ConfigureAwait(false);

            var newId = Directory.EnumerateFiles(directory)
                .Select(GetId)
                .DefaultIfEmpty()
                .Max() + 1;

            return newId;
        }

        private async Task<string> GetFilePathAsync(int id)
        {
            var directory = await GetDatabasePath()
                .ConfigureAwait(false);

            return Path.Combine(directory, $"{id}.json");
        }

        private Task<WorkspaceSettings> GetSettings() => _settingsProvider.GetWorkspaceSettings();

        private async Task<string?> TryGetDatabasePath()
        {
            var settings = await GetSettings()
                .ConfigureAwait(false);

            return !string.IsNullOrEmpty(settings.DatabasePath) && Directory.Exists(settings.DatabasePath)
                ? settings.DatabasePath
                : null;
        }

        private async Task<string> GetDatabasePath()
        {
            var settings = await GetSettings()
                .ConfigureAwait(false);

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
