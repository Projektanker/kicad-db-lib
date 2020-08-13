using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
    }
}
