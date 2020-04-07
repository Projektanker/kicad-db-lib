import { SettingsService } from '../settings/settings.service';
import { PartService } from '../parts/part.service';
import * as path from 'path';
import { KiCadLibraryBuilder } from '../kicad/kicad-library-builder';
import { KiCadLibraryReader } from '../kicad/kicad-library-reader';

export class LibraryService {
  private settingsService: SettingsService;
  private partService: PartService;
  private builder: KiCadLibraryBuilder;
  private reader: KiCadLibraryReader;

  symbols: string[] = null;
  footprints: string[] = null;

  constructor() {
    this.settingsService = new SettingsService();
    this.partService = new PartService();
    this.builder = new KiCadLibraryBuilder();
    this.reader = new KiCadLibraryReader();
  }

  public async build(clearBeforeBuild: boolean = true): Promise<any> {
    try {
      console.log('Build()');
      console.log('read parts');
      var parts = await this.partService.getParts();
      console.log('read settings');
      var settings = await this.settingsService.getSettings();
      console.log('prepare parts');
      for (let i = 0; i < parts.length; i++) {
        const part = parts[i];
        var split = part.symbol.split(':');
        if (split.length != 2) {
          throw new Error('Symbol must be defined as "library name:symbol"');
        }
        var library = split[0];
        var symbol = split[1];
        part.symbol = `${path.join(
          settings.paths.symbol,
          `${library}.lib`
        )}:${symbol}`;
        parts[i] = part;
      }
      console.log('build...');
      await this.builder.buildAsync(
        parts,
        settings.paths.output,
        clearBeforeBuild
      );
      console.log('done');
      return Promise.resolve();
    } catch (error) {
      console.error(error);
      return Promise.reject(error);
    }
  }

  private regExpEscape(s: string): string {
    /*
    Source:
    https://stackoverflow.com/questions/3561493/is-there-a-regexp-escape-function-in-javascript/3561711#3561711
     */
    return s.replace(/[-\/\\^$*+?.()|[\]{}]/g, '\\$&');
  }

  private wildCardToRegExp(s: string): string {
    var exp = this.regExpEscape(s)
      .replace(/\\\*/g, '.*?')
      .replace(/\\\?/g, '.?');

    return exp;
  }

  public async getSymbols(
    filter: string,
    reload: boolean,
    max?: number
  ): Promise<string[]> {
    try {
      if (!this.symbols || reload) {
        var settings = await this.settingsService.getSettings();
        var directory = settings.paths.symbol;
        this.symbols = await this.reader.getSymbolsFromDirectoryAsync(
          directory
        );
      }
      var regex = this.wildCardToRegExp(filter.toLowerCase().trim());
      var filtered = this.symbols
        .filter(s => s.toLowerCase().match(regex))
        .slice(0, max);
      return Promise.resolve(filtered);
    } catch (error) {
      return Promise.reject(error);
    }
  }

  public async getFootprints(
    filter: string,
    reload: boolean,
    max?: number
  ): Promise<string[]> {
    try {
      if (!this.footprints || reload) {
        var settings = await this.settingsService.getSettings();
        var directory = settings.paths.footprint;
        this.footprints = await this.reader.getFootprintsFromDirectoryAsync(
          directory
        );
      }
      var regex = this.wildCardToRegExp(filter.toLowerCase().trim());
      var filtered = this.footprints
        .filter(s => s.toLowerCase().match(regex))
        .slice(0, max);
      return Promise.resolve(filtered);
    } catch (error) {
      return Promise.reject(error);
    }
  }
}
