using System;
using KiCadDbLib.Models;

namespace KiCadDbLib.Services
{
    public class SettingsChangedEventArgs : EventArgs
    {
        public SettingsChangedEventArgs(WorkspaceSettings oldSettings, WorkspaceSettings newSettings)
        {
            OldSettings = oldSettings;
            NewSettings = newSettings;
        }

        public WorkspaceSettings NewSettings { get; }

        public WorkspaceSettings OldSettings { get; }
    }
}