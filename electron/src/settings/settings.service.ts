import { Settings } from '../../../shared/settings/settings';
import * as path from 'path';
import { fs } from '../fs';

export class SettingsService {
  async getSettings(): Promise<Settings> {
    var settings: Settings;
    var file = path.join(__dirname, '/settings.json');
    try {
      var data = await fs.readFile(file, 'utf8');
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
    var file = path.join(__dirname, '/settings.json');
    var data = JSON.stringify(settings);
    fs.writeFile(file, data, 'utf8');
  }
}
