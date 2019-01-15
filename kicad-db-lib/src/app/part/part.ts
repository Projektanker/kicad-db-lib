export class Part {
    id: number;

    // Schematic reference. E.g. "R" for resistors
    reference: string;

    // Part value. E.g. "10K". Must be unique per <see cref="Library"/>
    value: string;

    // Footprint of part. Have to match KiCad footprint libraries. E.g. "Resistor_SMD:R_0603_1608Metric"
    footprint: string;

    // Reference to symbol. ../lib1.lib:symbol1
    symbol: string;

    // Library the part have be stored in. <see cref="Value"/> must be unique per library.
    library: string;

    // Datasheet link
    datasheet: string;

    // Description of part.
    description: string;

    // Keywords to find part
    keywords: string;

    // Custom fields like manufacturer etc.
    customFields: { [name: string]: string };
}
