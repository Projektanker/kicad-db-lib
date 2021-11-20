using System;
using KiCadDbLib.Models;

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