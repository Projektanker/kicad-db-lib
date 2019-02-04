import { Settings } from '../../../shared/settings/settings';

import { fs } from '../fs';

export class SettingsService {
  async getSettings(fileName: string): Promise<Settings> {
    var settings: Settings;
    var exist = await fs.stat(fileName);

    if (!exist) {
      settings = new Settings();
    } else {
      var data = await fs.readFile(fileName, 'utf8');
      settings = JSON.parse(data);
    }

    return Promise.resolve(settings);
  }

  async updateSettings(fileName: string, settings: Settings) {
    var data = JSON.stringify(settings);
    fs.writeFile(fileName, data, 'utf8');
  }
}
