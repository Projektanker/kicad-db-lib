import { KiCadLibraryReader } from './kicad-library-reader';
import { Part } from '../parts/part';
import { fs } from '../fs';
import * as path from 'path';

export class KiCadLibraryBuilder {
  private readonly _reader: KiCadLibraryReader;

  constructor() {
    this._reader = new KiCadLibraryReader();
  }

  public async buildAsync(
    kiCadParts: Part[],
    outputDirectory: string,
    clearOutputDirectory: boolean = false
  ): Promise<any> {
    try {
      if (clearOutputDirectory) {
        var files = await fs.readdir(outputDirectory);
        files = files.filter(f => {
          return (
            f.toLowerCase().endsWith('.lib') || f.toLowerCase().endsWith('.dcm')
          );
        });

        for (let i = 0; i < files.length; i++) {
          const file = files[i];
          await fs.unlink(file);
        }
      }

      // Sort parts by Library and Value
      var parts = kiCadParts.sort((a, b) => {
        if (a.library == b.library) {
          return a.value < b.value ? -1 : a.value > b.value ? 1 : 0;
        } else {
          return a.library < b.library ? -1 : 1;
        }
      });

      var library: string = null;
      var libFile = null,
        dcmFile = null;
      var lib: string[] = null,
        dcm: string[] = null;

      for (let i = 0; i < parts.length; i++) {
        const part = parts[i];

        if (part.library != library) {
          // Close previous library files
          if (libFile != null) {
            lib.push('#End Library');
            await fs.writeFile(libFile, lib.join('\n'), 'utf8');
          }

          if (dcmFile != null) {
            dcm.push('#End Doc Library');
            await fs.writeFile(dcmFile, dcm.join('\n'), 'utf8');
          }

          // Create new library files
          library = part.library;
          lib = [];
          libFile = path.join(outputDirectory, `${library}.lib`);
          dcm = [];
          dcmFile = path.join(outputDirectory, `${library}.dcm`);

          // Write file head
          lib.push('EESchema-LIBRARY Version 2.4');
          lib.push('#encoding utf-8');
          lib.push('#');

          dcm.push('EESchema-DOCLIB  Version 2.0');
          dcm.push('#');
        }

        var template = await this._reader.getSymbolTemplateAsync(part);

        // Fill part and remove empty lines
        var values = [
          part.value,
          part.reference,
          part.footprint,
          part.datasheet,
          this.createCustomFields(part.customFields)
        ];
        var partString = template;
        for (let i = 0; i < values.length; i++) {
          const value = values[i];
          partString = partString.replace(`{${i}}`, value);
        }
        partString = partString.replace('\n\n', '\n');

        // Write .lib file
        lib.push(partString);

        // Write .dcm file
        if (part.value) dcm.push(`$CMP ${part.value}`);
        if (part.description) dcm.push(`D ${part.description}`);
        if (part.keywords) dcm.push(`K ${part.keywords}`);
        if (part.datasheet) dcm.push(`F ${part.datasheet}`);

        dcm.push('$ENDCMP');
        dcm.push('#');
        return Promise.resolve();
      }
    } catch (error) {
      return Promise.reject(error);
    }
  }
  private createCustomField(
    fieldNumber: number,
    key: string,
    value: string
  ): string {
    if (fieldNumber < 4) {
      throw new Error('Fieldnumber must be greater or equal 4!');
    }

    return `F${fieldNumber} \"${value}\" ${100 * (fieldNumber - 3)} ${100 *
      (fieldNumber - 2)} 50 H I C CNN \"${key}\"`;
  }

  private createCustomFields(fields: { [name: string]: string }): string {
    if (fields == null) return '';

    var text: string[] = [];
    var i = 4;

    for (const key in fields) {
      if (fields.hasOwnProperty(key)) {
        const value = fields[key];
        text.push(this.createCustomField(i, key, value));
        i++;
      }
    }

    return text.join('\n');
  }
}
