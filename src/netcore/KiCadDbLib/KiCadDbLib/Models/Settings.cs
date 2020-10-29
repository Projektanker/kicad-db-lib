using System.Collections.Generic;

namespace KiCadDbLib.Models
{
    public class Settings
    {
        public Settings()
        {
            CustomFields = new List<string>();
            DatabasePath = nameof(DatabasePath);
            FootprintsPath = nameof(FootprintsPath);
            OutputPath = nameof(OutputPath);
            SymbolsPath = nameof(SymbolsPath);
        }

        public IList<string> CustomFields { get; }

        public string DatabasePath { get; set; }

        public string FootprintsPath { get; set; }

        public string OutputPath { get; set; }

        public string SymbolsPath { get; set; }
    }
}