using System;
using System.IO;

namespace KiCadDbLib.Services
{
    public static class DirectoryProvider
    {
        public static string ApplicationData => Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "kicad-db-lib");
    }
}