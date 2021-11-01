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
}