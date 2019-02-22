import { fs } from '../fs';
import { Part } from '../part/part';
import * as path from 'path';
import { SettingsService } from '../settings/settings.service';
import { on } from 'cluster';

export class PartService {
  libraries: string[] = null;

  async getParts(
    sortActive: string = '',
    sortDirection: string = ''
  ): Promise<Part[]> {
    try {
      var settingsService = new SettingsService();
      var settings = await settingsService.getSettings();
      var files = await fs.readdir(settings.paths.parts);
      var parts: Part[] = [];
      for (var i = 0; i < files.length; i++) {
        var file = path.join(settings.paths.parts, files[i]);
        console.log(file);
        var json = await fs.readFile(file, 'utf8');
        var part: Part = JSON.parse(json);
        console.log(part);
        parts.push(part);
      }
      // sort parts
      parts = this.sortParts(parts, sortActive, sortDirection);
      return Promise.resolve(parts);
    } catch (error) {
      console.error(error);
      return Promise.resolve([]);
    }
  }

  private sortParts(
    parts: Part[],
    sortActive: string = '',
    sortDirection: string = ''
  ): Part[] {
    if (!sortActive || !sortDirection) {
      parts.sort((a, b) => a.id - b.id);
    } else {
      var asc: boolean = sortDirection == 'asc';
      switch (sortActive) {
        case 'id':
          parts.sort((a, b) => (asc ? a.id - b.id : b.id - a.id));
          break;

        default:
          parts.sort((a: Part, b: Part) => {
            var key1 = a.hasOwnProperty(sortActive)
              ? (a as any)[sortActive]
              : a.customFields[sortActive];
            var key2 = b.hasOwnProperty(sortActive)
              ? (b as any)[sortActive]
              : b.customFields[sortActive];
            if (key1 == key2) {
              return 0;
            }
            if (key1 < key2) {
              return asc ? -1 : 1;
            }
            return asc ? 1 : -1;
          });
          break;
      }
    }
    return parts;
  }

  async getPart(id: number): Promise<Part> {
    try {
      console.log(`getPart(${id})`);
      var settingsService = new SettingsService();
      var settings = await settingsService.getSettings();
      var file = path.join(settings.paths.parts, `${id}.json`);
      console.log(file);
      var part: Part = null;
      var json = await fs.readFile(file, 'utf8');
      var part: Part = JSON.parse(json);
      console.log(part);
      return Promise.resolve(part);
    } catch (error) {
      console.error(error);
      var part = new Part();
      part.id = id;
      return Promise.resolve(part);
    }
  }

  async addPart(part: Part): Promise<Part> {
    try {
      console.log(`addPart(part: Part)`);
      var settingsService = new SettingsService();
      var settings = await settingsService.getSettings();
      var files = await fs.readdir(settings.paths.parts);
      var allIds = files.map(f => parseInt(f)).filter(n => !isNaN(n));
      part.id = Math.max(...allIds) + 1;
      var file = path.join(settings.paths.parts, `${part.id}.json`);
      console.log(file);
      console.log(part);
      var json = JSON.stringify(part);
      await fs.writeFile(file, json, 'utf8');
      return Promise.resolve(part);
    } catch (error) {
      console.error(error);
      return Promise.reject(error);
    }
  }

  async updatePart(part: Part): Promise<Part> {
    try {
      console.log(`updatePart(part: Part)`);
      var settingsService = new SettingsService();
      var settings = await settingsService.getSettings();
      var file = path.join(settings.paths.parts, `${part.id}.json`);
      console.log(file);
      console.log(part);
      var json = JSON.stringify(part);
      await fs.writeFile(file, json, 'utf8');
      return Promise.resolve(part);
    } catch (error) {
      console.error(error);
      return Promise.reject(error);
    }
  }

  async deletePart(part: Part | number): Promise<any> {
    try {
      console.log(`deletePart(part: Part | number)`);
      const id = typeof part === 'number' ? part : part.id;
      var settingsService = new SettingsService();
      var settings = await settingsService.getSettings();
      var file = path.join(settings.paths.parts, `${id}.json`);
      console.log(file);
      console.log(part);
      await fs.unlink(file);
      return Promise.resolve();
    } catch (error) {
      console.error(error);
      return Promise.reject(error);
    }
  }

  async getLibraries(
    filter: string,
    reload: boolean,
    max?: number
  ): Promise<string[]> {
    try {
      if (!this.libraries || reload) {
        var parts = await this.getParts();
        this.libraries = parts
          .map(p => p.library)
          .filter((value, index, array) => array.indexOf(value) === index)
          .sort();
      }
      var filtered = this.libraries
        .filter(l => l.toLowerCase().includes(filter.toLowerCase()))
        .slice(0, max);
      return Promise.resolve(filtered);
    } catch (error) {
      return Promise.reject(error);
    }
  }
}
