﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KiCadDbLib.Models;
using Newtonsoft.Json;

namespace KiCadDbLib.Services
{
    public class PartsService
    {
        private readonly SettingsService _settingsService;

        public PartsService(SettingsService settingsService)
        {
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
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

        public async Task DeleteAsync(string id)
        {
            string filePath = await GetFilePathAsync(id);
            File.Delete(filePath);
        }

        public async Task AddOrUpdateAsync(Part part)
        {
            if (part is null)
            {
                throw new ArgumentNullException(nameof(part));
            }

            string filePath = await GetFilePathAsync(part.Id);

            await Task.Run(() =>
            {
                JsonSerializer serializer = JsonSerializer.Create();
                using StreamWriter fileWriter = File.CreateText(filePath);
                using JsonTextWriter jsonWriter = new JsonTextWriter(fileWriter);
                serializer.Serialize(jsonWriter, part);
            });            
        }

        private async Task<string> GetFilePathAsync(string id)
        {
            var settings = await _settingsService.GetSettingsAsync();
            return Path.Combine(settings.DatabasePath, $"{id}.json");
        }
    }
}