import * as oldFs from 'fs';
import * as util from 'util';

export abstract class fs {
  public static stat = util.promisify(oldFs.stat);
  public static readFile = util.promisify(oldFs.readFile);
  public static writeFile = util.promisify(oldFs.writeFile);
}
