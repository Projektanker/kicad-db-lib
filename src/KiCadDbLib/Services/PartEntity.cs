using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using KiCadDbLib.Models;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

public class PartEntity
{
    public string Library { get; init; } = string.Empty;
    public string Reference { get; init; } = string.Empty;
    public string Symbol { get; init; } = string.Empty;
    public string Value { get; init; } = string.Empty;
    public string Footprint { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Datasheet { get; init; } = string.Empty;
    public string Keywords { get; init; } = string.Empty;

    public IReadOnlyDictionary<string, string> CustomFields { get; init; }
        = new Dictionary<string, string>();

    public static PartEntity From(Part part)
    {
        return new PartEntity
        {
            Library = part.Library,
            Reference = part.Reference,
            Symbol = part.Symbol,
            Value = part.Value,
            Footprint = part.Footprint,
            Description = part.Description,
            Datasheet = part.Datasheet,
            Keywords = part.Keywords,
            CustomFields = part.CustomFields,
        };
    }

    public Part ToPart(int id)
    {
        return new Part
        {
            Id = id,
            Library = Library,
            Reference = Reference,
            Symbol = Symbol,
            Value = Value,
            Footprint = Footprint,
            Description = Description,
            Datasheet = Datasheet,
            Keywords = Keywords,
            CustomFields = CustomFields,
        };
    }
}