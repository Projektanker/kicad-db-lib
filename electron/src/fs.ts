import * as oldFs from 'fs';
import * as util from 'util';

export abstract class fs {
  public static readonly stat = util.promisify(oldFs.stat);
  public static readonly readFile = util.promisify(oldFs.readFile);
  public static readonly writeFile = util.promisify(oldFs.writeFile);
  public static readonly readdir = util.promisify(oldFs.readdir);
}
