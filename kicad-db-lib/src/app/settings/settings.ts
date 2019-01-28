export class Settings {
  customFields: string[] = [];
  paths: { [name: string]: string };
  test: { [name: string]: string } = {
    symbol: null,
    footprint: null,
    output: null
  };
}
