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

  public async getSymbols(filter: string = ''): Promise<string[]> {
    try {
      var settings = await this.settingsService.getSettings();
      var directory = settings.paths.symbol;
      var symbols = await this.reader.getSymbolsFromDirectoryAsync(directory);
      symbols = symbols.filter(s => s.includes(filter));
      return Promise.resolve(symbols);
    } catch (error) {
      return Promise.reject(error);
    }
  }

  public async getFootprints(filter: string = ''): Promise<string[]> {
    try {
      var settings = await this.settingsService.getSettings();
      var directory = settings.paths.footprint;
      var footprint = await this.reader.getFootprintsFromDirectoryAsync(
        directory
      );
      footprint = footprint.filter(s => s.includes(filter));
      return Promise.resolve(footprint);
    } catch (error) {
      return Promise.reject(error);
    }
  }
}
