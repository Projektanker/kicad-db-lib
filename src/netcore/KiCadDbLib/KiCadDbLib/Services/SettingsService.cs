using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using KiCadDbLib.Models;
using Newtonsoft.Json;
using Splat;

namespace KiCadDbLib.Services
{
    public class SettingsChangedEventArgs : EventArgs
    {
        public SettingsChangedEventArgs(Settings oldSettings, Settings newSettings)
        {
            OldSettings = oldSettings;
            NewSettings = newSettings;
        }

        public Settings NewSettings { get; }
        public Settings OldSettings { get; }
    }

    public class SettingsService
    {
        private readonly object _settingsLock;
        private Settings _settings;

        public SettingsService()
        {
            _settingsLock = new object();
            Location = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                Assembly.GetExecutingAssembly().GetName().Name,
                "settings.json");
        }

        public event EventHandler<SettingsChangedEventArgs> SettingsChanged;

        public string Location { get; }

        public Task<Settings> GetSettingsAsync()
        {
            lock (_settingsLock)
            {
                return Task.Run(() => GetSettings());
            }
        }

        public async Task SetSettingsAsync(Settings settings)
        {
            await Task.Run(() => SetSettings(settings));
        }

        private Settings GetSettings()
        {
            lock (_settingsLock)
            {
                if (_settings != null)
                {
                    return _settings;
                }
                else if (File.Exists(Location))
                {
                    string json = File.ReadAllText(Location);
                    return _settings = JsonConvert.DeserializeObject<Settings>(json);
                }
                else
                {
                    return _settings = new Settings();
                }
            }
        }

        private void SetSettings(Settings settings)
        {
            if (settings is null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            lock (_settingsLock)
            {
                FileInfo file = new FileInfo(Location);
                file.Directory.Create();
                string json = JsonConvert.SerializeObject(settings);
                File.WriteAllText(file.FullName, json);

                var oldSettings = _settings;
                _settings = settings;

                SettingsChanged?.Invoke(
                    this,
                    new SettingsChangedEventArgs(oldSettings, _settings));
            }
        }
    }
}