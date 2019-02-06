import { fs } from '../fs';
import { Part } from '../parts/part';

export class KiCadLibraryReader {
  private _getSymbol_library: string;
  private _getSymbol_lines: string[];

  public async getSymbolAsync(
    library: string,
    symbol: string,
    bufferLibrary: boolean = true
  ): Promise<string> {
    try {
      if (!fs.checkFileExists(library)) {
        throw new Error(`Symbol library \"${library}\" not found!`);
      }

      var lines: string[];

      if (bufferLibrary && library == this._getSymbol_library) {
        lines = this._getSymbol_lines;
      } else {
        var text = await fs.readFile(library, 'utf8');
        lines = text.replace('\r\n', '\n').split('\n');
        this._getSymbol_library = bufferLibrary ? library : null;
        this._getSymbol_lines = bufferLibrary ? lines : null;
      }

      var start = 0,
        end = 0;
      // Search begin of symbol (DEF ...)
      for (var i = 0; i < lines.length; i++) {
        const line = lines[i];
        if (!line.startsWith(`DEF ${symbol}`)) continue;

        start = i;
        break;
      }

      if (start == 0) {
        throw new Error(
          `Symbol \"${symbol}\" not found in symbol library \"${library}\"`
        );
      }
      // Search end of symbol (ENDDEF)
      for (var i = start + 1; i < lines.length; i++) {
        var line = lines[i];
        if (!line.startsWith('ENDDEF')) continue;

        end = i + 1;
        break;
      }

      if (end < start) {
        throw new Error(
          `Symbol library \"${library}\" is corrupted! \"ENDDEF\" not found!`
        );
      }

      // Return symbol
      var stringBuilder: string[] = [];
      // Add comment before symbol
      stringBuilder.push(`"# ${symbol}`);
      stringBuilder.push('#');

      for (i = start; i < end; i++) {
        stringBuilder.push(lines[i]);
      }
      // Add comment after symbol
      stringBuilder.push('#');

      return Promise.resolve(stringBuilder.join('\n'));
    } catch (error) {
      return Promise.reject(error);
    }
  }

  public async getSymbolTemplateAsync(part: Part): Promise<string> {
    try {
      var split = part.symbol.split(':');
      if (split.length < 2) {
        throw new Error('Symbol must be defined as "path:symbol"');
      }

      var symbol = split[split.length - 1];
      split.pop();
      var path = split.join(':');

      // Get symbol
      var template = await this.getSymbolAsync(path, symbol);

      // Normalize new line, split on new line and remove empty lines
      var lines = template
        .replace('\r\n', '\n')
        .split('\n')
        .filter(l => {
          return l;
        });
      // Remove lines starting with # ( and add them later )
      while (lines[0].startsWith('#')) {
        lines.shift();
      }

      // Prepare for string.Format()
      let i = 0;
      var format, startIndex, index;

      var start: string;
      var find;
      // "DEF value reference ..." to "DEF {0} {1} ..."
      start = 'DEF ';
      format = 0;
      find = ' ';
      startIndex = start.length;
      index = lines[i].indexOf(find, startIndex);
      lines[i] =
        lines[i].substr(0, startIndex) + `{${format}}` + lines[i].substr(index);
      start = 'DEF {0} ';
      format = 1;
      startIndex = start.length;
      index = lines[i].indexOf(find, startIndex);
      lines[i] =
        lines[i].substr(0, startIndex) + `{${format}}` + lines[i].substr(index);

      // Clear fields F0 - F3
      start = 'Fx "';
      find = '"';
      for (i = 1; i < 5; i++) {
        switch (i) {
          case 1:
            format = 1;
            break;
          case 2:
            format = 0;
            break;
          case 3:
            format = 2;
            break;
          case 4:
            format = 3;
            break;
          default:
            break;
        }
        startIndex = start.length;
        index = lines[i].indexOf(find, startIndex);
        lines[i] =
          lines[i].substr(0, startIndex) +
          `{${format}}` +
          lines[i].substr(index);
      }

      // Remove F4 and following
      while (lines[i].startsWith('F')) {
        lines.splice(i, 1);
      }

      // Insert {4} for custom fields
      lines.splice(i, 0, '{4}');

      // Find and remove ALIAS
      for (i = 0; i < lines.length; i++) {
        if (!lines[i].startsWith('ALIAS')) continue;

        lines.splice(i, 1);
        break;
      }

      // Restore deleted #-Lines
      lines.splice(0, 0, '# {0}');
      lines.splice(1, 0, '#');

      return Promise.resolve(lines.join('\n'));
    } catch (error) {
      return Promise.reject(error);
    }
  }

  public async getSymbolsAsync(library: string): Promise<string[]> {
    try {
      if (!fs.checkFileExists(library)) {
        throw new Error(`File \"${library}\" not found!`);
      }

      var symbols = [];
      var text = await fs.readFile(library, 'utf8');
      var lines = text.replace('\r\n', '\n').split('\n');
      for (let i = 0; i < lines.length; i++) {
        const line = lines[i];
        if (!line.startsWith('DEF')) continue;
        var start, end;
        start = 'DEF '.length;
        end = line.indexOf(' ', start);
        symbols.push(line.substring(start, end));
      }

      return Promise.resolve(symbols);
    } catch (error) {
      Promise.reject(error);
    }
  }
}
