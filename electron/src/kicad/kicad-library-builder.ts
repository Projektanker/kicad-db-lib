import { KiCadLibraryReader } from './kicad-library-reader';
import { Part } from '../part/part';
import { fs } from '../fs';
import * as path from 'path';

export class KiCadLibraryBuilder {
  private readonly _reader: KiCadLibraryReader;

  constructor() {
    this._reader = new KiCadLibraryReader();
  }

  private async finishLibrary(
    outputDirectory: string,
    library: string,
    lib: string[],
    dcm: string[]
  ): Promise<any> {
    if (!lib) return;
    if (!dcm) return;

    var libFile = path.join(outputDirectory, `${library}.lib`);
    var dcmFile = path.join(outputDirectory, `${library}.dcm`);

    lib.push('#End Library\n');
    var libText = lib.join('\n');
    console.log(libFile);
    await fs.writeFile(libFile, libText, 'utf8');

    dcm.push('#End Doc Library\n');
    var dxmText = dcm.join('\n');
    console.log(dcmFile);
    await fs.writeFile(dcmFile, dxmText, 'utf8');
  }

  public async buildAsync(
    kiCadParts: Part[],
    outputDirectory: string,
    clearOutputDirectory: boolean = false
  ): Promise<any> {
    try {
      console.log('buildAsync()');

      if (clearOutputDirectory) {
        console.log('clear output directory:');
        var files = await fs.readdir(outputDirectory);
        files = files
          .filter(f => {
            return (
              f.toLowerCase().endsWith('.lib') ||
              f.toLowerCase().endsWith('.dcm')
            );
          })
          .map(f => path.join(outputDirectory, f));

        for (let i = 0; i < files.length; i++) {
          const file = files[i];
          await fs.unlink(file);
        }
      }

      // Sort parts by Library and Value
      console.log('Sort parts by Library and Value');
      var parts = kiCadParts.sort((a, b) => {
        if (a.library == b.library) {
          return a.value < b.value ? -1 : a.value > b.value ? 1 : 0;
        } else {
          return a.library < b.library ? -1 : 1;
        }
      });

      var library: string = null;
      var lib: string[] = null,
        dcm: string[] = null;

      for (let i = 0; i < parts.length; i++) {
        console.log(`part[${i}]`);
        const part = parts[i];

        if (part.library != library) {
          // Close previous library files
          await this.finishLibrary(outputDirectory, library, lib, dcm);

          // Create new library files
          library = part.library;
          lib = [];
          dcm = [];

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
          var find = new RegExp(`\\{${i}\\}`, 'g'); // resolves in: /\{0\}/g
          partString = partString.replace(find, value);
        }
        partString = partString.replace(/\n\n/g, '\n');

        // Write .lib file
        lib.push(partString);

        // Write .dcm file
        if (part.value) dcm.push(`$CMP ${part.value}`);
        if (part.description) dcm.push(`D ${part.description}`);
        if (part.keywords) dcm.push(`K ${part.keywords}`);
        if (part.datasheet) dcm.push(`F ${part.datasheet}`);

        dcm.push('$ENDCMP');
        dcm.push('#');
      }
      // Close previous library files
      await this.finishLibrary(outputDirectory, library, lib, dcm);

      return Promise.resolve();
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
