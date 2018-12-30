namespace KiCadDbLib.Model
{
    class KiCadPart
    {
        public string Identifier { get; set; }
        public string Reference { get; set; }
        // Unique per .lib 
        public string Value { get; set; }
        public string Footprint { get; set; }
        public string Symbol { get; set; }
        public string Library { get; set; }
        public string Datasheet { get; set; }

        // From Properties
        public string Description { get; set; }
        public string Keywords { get; set; }

        public Dictonary<string, string> Fields { get; set; }
    }
}