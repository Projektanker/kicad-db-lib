import { fs } from '../fs';
import { Part } from './part';
import * as path from 'path';
import { SettingsService } from '../settings/settings.service';

export class PartService {
  async getParts(): Promise<Part[]> {
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
      return Promise.resolve(parts);
    } catch (error) {
      console.error(error);
      return Promise.resolve([]);
    }
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
      part.id = files.length + 1;
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

  async getLibraries(): Promise<string[]> {
    try {
      var parts = await this.getParts();
      var libraries = parts
        .map(p => p.library)
        .filter((value, index, array) => array.indexOf(value) === index)
        .sort();
      return Promise.resolve(libraries);
    } catch (error) {
      return Promise.reject(libraries);
    }
  }
}
