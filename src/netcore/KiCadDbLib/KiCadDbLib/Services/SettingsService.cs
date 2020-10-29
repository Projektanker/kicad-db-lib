using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using KiCadDbLib.Models;
using Newtonsoft.Json;
using Splat;

namespace KiCadDbLib.Services
{
    public class SettingsService
    {
        private Settings _settings;

        public SettingsService()
        {
            Location = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                Assembly.GetExecutingAssembly().GetName().Name,
                "settings.json");
        }

        public event EventHandler<SettingsChangedEventArgs> SettingsChanged;

        public string Location { get; }

        public async Task<Settings> GetSettingsAsync()
        {
            if (_settings != null)
            {
                return _settings;
            }
            else if (File.Exists(Location))
            {
                string json = await File.ReadAllTextAsync(Location).ConfigureAwait(false);
                return _settings = JsonConvert.DeserializeObject<Settings>(json);
            }
            else
            {
                return _settings = new Settings();
            }
        }

        public async Task SetSettingsAsync(Settings settings)
        {
            if (settings is null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            FileInfo file = new FileInfo(Location);
            file.Directory.Create();
            string json = JsonConvert.SerializeObject(settings);
            await File.WriteAllTextAsync(file.FullName, json).ConfigureAwait(false);

            var oldSettings = _settings;
            _settings = settings;

            SettingsChanged?.Invoke(
                this,
                new SettingsChangedEventArgs(oldSettings, _settings));
        }
    }
}