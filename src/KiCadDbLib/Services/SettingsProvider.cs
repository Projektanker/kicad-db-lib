using System;
using System.IO;
using System.Threading.Tasks;
using KiCadDbLib.Models;
using Newtonsoft.Json;

namespace KiCadDbLib.Services
{
    public class SettingsProvider : ISettingsProvider
    {
        public SettingsProvider()
        {
            Location = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "kicad-db-lib",
                "settings.json");
        }

        public string Location { get; }

        public async Task<Settings> GetSettingsAsync()
        {
            if (File.Exists(Location))
            {
                string json = await File.ReadAllTextAsync(Location).ConfigureAwait(false);
                return JsonConvert.DeserializeObject<Settings>(json)!;
            }
            else
            {
                return new Settings();
            }
        }

        public async Task SetSettingsAsync(Settings settings)
        {
            var file = new FileInfo(Location);
            file.Directory!.Create();

            string json = JsonConvert.SerializeObject(settings);
            await File.WriteAllTextAsync(file.FullName, json).ConfigureAwait(false);
        }
    }
}