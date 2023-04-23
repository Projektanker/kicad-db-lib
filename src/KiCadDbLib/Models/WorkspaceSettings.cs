using System.Collections.Generic;

namespace KiCadDbLib.Models
{
    public class WorkspaceSettings
    {
        public IList<string> CustomFields { get; init; } = new List<string>();

        public string DatabasePath { get; set; } = "parts";

        public string FootprintsPath { get; set; } = "footprints";
        public string SymbolsPath { get; set; } = "symbols";

        public string OutputPath { get; set; } = "output";
    }
}