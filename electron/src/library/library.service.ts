import { SettingsService } from '../settings/settings.service';
import { PartService } from '../parts/part.service';
import * as path from 'path';
import { KiCadLibraryBuilder } from '../kicad/kicad-library-builder';

export class LibraryService {
  private settingsService: SettingsService;
  private partService: PartService;
  private builder: KiCadLibraryBuilder;

  constructor() {
    this.settingsService = new SettingsService();
    this.partService = new PartService();
    this.builder = new KiCadLibraryBuilder();
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
}
