export class Settings {
  customFields: string[] = [];
  paths: SettingsPaths = new SettingsPaths();
}

export class SettingsPaths {
  parts: string;
  symbol: string;
  footprint: string;
  output: string;
}
