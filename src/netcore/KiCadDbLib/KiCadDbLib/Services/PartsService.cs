using System;
using System.Collections.Generic;
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

        private LibraryItemInfo[] _cachedFootprints;

        private LibraryItemInfo[] _cachedSymbols;

        public PartsService(SettingsService settingsService)
        {
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            _settingsService.SettingsChanged += SettingsService_SettingsChanged;
        }

        public async Task AddOrUpdateAsync(Part part)
        {
            if (part is null)
            {
                throw new ArgumentNullException(nameof(part));
            }

            var settings = await _settingsService.GetSettingsAsync();

            if (string.IsNullOrEmpty(part.Id))
            {
                part.Id = await GetNewId(settings);
            }

            string filePath = await GetFilePathAsync(part.Id, settings);

            await Task.Run(() =>
            {
                JsonSerializer serializer = JsonSerializer.Create();
                using StreamWriter fileWriter = File.CreateText(filePath);
                using JsonTextWriter jsonWriter = new JsonTextWriter(fileWriter);
                serializer.Serialize(jsonWriter, part);
            });
        }

        public async Task Build()
        {
            var settings = await _settingsService.GetSettingsAsync();
            var parts = await GetPartsAsync();

            await KiCadLibraryBuilder.ClearDirectoryAsync(settings.OutputPath);

            var groupedParts = parts
                 .GroupBy(part => part.Library);

            foreach (IGrouping<string, Part> group in groupedParts)
            {
                using var builder = new KiCadLibraryBuilder(settings.OutputPath, group.Key);
                await builder.WriteStartLibrary();

                foreach (var part in group.OrderBy(part => part.Value))
                {
                    LibraryItemInfo symbolInfo = LibraryItemInfo.Parse(part.Symbol);
                    symbolInfo.Library = Path.Combine(
                        settings.SymbolsPath,
                        symbolInfo.Library + FileExtensions.Lib);

                    await builder.WritePartAsync(
                        reference: part.Reference,
                        value: part.Value,
                        symbol: symbolInfo,
                        footprint: part.Footprint,
                        description: part.Description,
                        keywords: part.Keywords,
                        datasheet: part.Datasheet,
                        customFields: part.CustomFields);
                }

                await builder.WriteEndLibrary();
            }
        }

        public async Task DeleteAsync(string id)
        {
            string filePath = await GetFilePathAsync(id);
            File.Delete(filePath);
        }

        public async Task<LibraryItemInfo[]> GetFootprintsAsync()
        {
            if (_cachedFootprints is null)
            {

                var settings = await _settingsService.GetSettingsAsync();
                _cachedFootprints = await new KiCadLibraryReader()
                    .GetFootprintInfosFromDirectoryAsync(settings.FootprintsPath);
            }

            return _cachedFootprints;
        }

        public async Task<string> GetNewId(Settings settings = null)
        {
            settings ??= await _settingsService.GetSettingsAsync();
            int newId = Directory.EnumerateFiles(settings.DatabasePath)
                .Select(file => Path.GetFileNameWithoutExtension(file))
                .Select(id => int.TryParse(id, out int result) ? result : default)
                .DefaultIfEmpty()
                .Max() + 1;

            return newId.ToString();
        }

        public async Task<Part[]> GetPartsAsync()
        {
            var settings = await _settingsService.GetSettingsAsync();

            JsonSerializer serializer = JsonSerializer.Create();
            IEnumerable<Task<Part>> tasks = Directory.EnumerateFiles(settings.DatabasePath)
                .Select(file => Task.Run(() =>
                {
                    using StreamReader fileReader = File.OpenText(file);
                    using JsonTextReader jsonReader = new JsonTextReader(fileReader);
                    Part part = serializer.Deserialize<Part>(jsonReader);
                    return part;
                }));

            Part[] parts = await Task.WhenAll(tasks);
            return parts;
        }

        public async Task<LibraryItemInfo[]> GetSymbolsAsync()
        {
            if (_cachedSymbols is null)
            {

                var settings = await _settingsService.GetSettingsAsync();
                _cachedSymbols = await new KiCadLibraryReader()
                    .GetSymbolInfosFromDirectoryAsync(settings.SymbolsPath);
            }

            return _cachedSymbols;
        }

        private async Task<string> GetFilePathAsync(string id, Settings settings = null)
        {
            settings ??= await _settingsService.GetSettingsAsync();
            return Path.Combine(settings.DatabasePath, $"{id}.json");
        }

        private void SettingsService_SettingsChanged(object sender, SettingsChangedEventArgs e)
        {
            if (e.OldSettings?.SymbolsPath != e.NewSettings?.SymbolsPath)
            {
                _cachedSymbols = null;
            }

            if (e.OldSettings.FootprintsPath != e.NewSettings?.SymbolsPath)
            {
                _cachedFootprints = null;
            }
        }
    }
}