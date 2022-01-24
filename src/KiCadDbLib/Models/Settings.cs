using System.Collections.Generic;

namespace KiCadDbLib.Models
{
    public class Settings
    {
        public IList<string> CustomFields { get; } = new List<string>();

        public string DatabasePath { get; set; } = string.Empty;

        public string FootprintsPath { get; set; } = string.Empty;

        public string OutputPath { get; set; } = string.Empty;

        public string SymbolsPath { get; set; } = string.Empty;
    }
}