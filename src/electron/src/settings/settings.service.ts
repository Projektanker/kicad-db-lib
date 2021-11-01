import { Settings } from './settings';
import * as path from 'path';
import { fs } from '../fs';
import { app } from 'electron';

export class SettingsService {
  private file: string;

  constructor() {
    this.file = path.join(app.getPath('userData'), 'settings.json');
  }

  async getSettings(): Promise<Settings> {
    var settings: Settings;
    try {
      var data = await fs.readFile(this.file, 'utf8');
      settings = JSON.parse(data);
    } catch (error) {
      settings = new Settings();
      settings.customFields = ['OC_FARNELL', 'OC_MOUSER'];
      settings.paths.footprint = 'footprint';
      settings.paths.output = 'output';
      settings.paths.parts = 'parts';
      settings.paths.symbol = 'symbol';
    }
    return Promise.resolve(settings);
  }

  async updateSettings(settings: Settings) {
    var data = JSON.stringify(settings);
    fs.writeFile(this.file, data, 'utf8');
  }
}
