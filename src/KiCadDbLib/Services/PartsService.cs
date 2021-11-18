using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KiCadDbLib.Models;
using KiCadDbLib.Services.KiCad;
using Newtonsoft.Json;

namespace KiCadDbLib.Services
{
    public class PartsService
    {
        private readonly SettingsService _settingsService;

        private LibraryItemInfo[]? _cachedFootprints;

        private LibraryItemInfo[]? _cachedSymbols;

        public PartsService(SettingsService settingsService)
        {
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            _settingsService.SettingsChanged += SettingsService_SettingsChanged;
        }

        public async Task AddOrUpdateAsync(Part part)
        {
            var settings = await _settingsService.GetSettingsAsync()
                .ConfigureAwait(false);

            if (string.IsNullOrEmpty(part.Id))
            {
                part.Id = await GetNewId(settings).ConfigureAwait(false);
            }

            var filePath = await GetFilePathAsync(part.Id, settings).ConfigureAwait(false);
            var json = JsonConvert.SerializeObject(part);
            await File.WriteAllTextAsync(filePath, json).ConfigureAwait(false);
        }

        public async Task Build()
        {
            var settings = await _settingsService.GetSettingsAsync().ConfigureAwait(false);
            var parts = await GetPartsAsync().ConfigureAwait(false);

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

        public async Task DeleteAsync(string id)
        {
            var filePath = await GetFilePathAsync(id).ConfigureAwait(false);
            File.Delete(filePath);
        }

        public async Task<LibraryItemInfo[]> GetFootprintsAsync()
        {
            if (_cachedFootprints is null)
            {
                var settings = await _settingsService.GetSettingsAsync().ConfigureAwait(false);
                _cachedFootprints = await KiCadLibraryReader
                    .GetFootprintInfosFromDirectoryAsync(settings.FootprintsPath)
                    .ConfigureAwait(false);
            }

            return _cachedFootprints;
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
            var settings = await _settingsService.GetSettingsAsync().ConfigureAwait(false);
            var filePath = await GetFilePathAsync(id, settings).ConfigureAwait(false);
            return await GetPartAsync(filePath, settings).ConfigureAwait(false);
        }

        public async Task<Part[]> GetPartsAsync()
        {
            var settings = await _settingsService.GetSettingsAsync().ConfigureAwait(false);

            IEnumerable<Task<Part>> tasks = Directory.EnumerateFiles(settings.DatabasePath)
                .Select(file => GetPartAsync(file, settings));

            Part[] parts = await Task.WhenAll(tasks).ConfigureAwait(false);
            return parts;
        }

        public async Task<LibraryItemInfo[]> GetSymbolsAsync()
        {
            if (_cachedSymbols is null)
            {
                var settings = await _settingsService.GetSettingsAsync().ConfigureAwait(false);
                _cachedSymbols = await KiCadLibraryReader
                    .GetSymbolInfosFromDirectoryAsync(settings.SymbolsPath)
                    .ConfigureAwait(false);
            }

            return _cachedSymbols;
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
            settings ??= await _settingsService.GetSettingsAsync().ConfigureAwait(false);
            var newId = Directory.EnumerateFiles(settings.DatabasePath)
                .Select(Path.GetFileNameWithoutExtension)
                .Select(id => int.TryParse(id, out var result) ? result : default)
                .DefaultIfEmpty()
                .Max() + 1;

            return newId.ToString(CultureInfo.InvariantCulture);
        }

        private async Task<string> GetFilePathAsync(string id, Settings? settings = null)
        {
            settings ??= await _settingsService.GetSettingsAsync().ConfigureAwait(false);
            return Path.Combine(settings.DatabasePath, $"{id}.json");
        }

        private void SettingsService_SettingsChanged(object? sender, SettingsChangedEventArgs e)
        {
            if (e.OldSettings?.SymbolsPath != e.NewSettings?.SymbolsPath)
            {
                _cachedSymbols = null;
            }

            if (e.OldSettings?.FootprintsPath != e.NewSettings?.SymbolsPath)
            {
                _cachedFootprints = null;
            }
        }
    }
}