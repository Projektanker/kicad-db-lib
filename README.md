# KiCad-Db-Lib

- [KiCad-Db-Lib](#kicad-db-lib)
  - [About](#about)
  - [Usage](#usage)
    - [Settings](#settings)
      - [Fields:](#fields)
      - [Paths](#paths)
    - [Add, update, delete part](#add-update-delete-part)
  - [Roadmap](#roadmap)

## About

Inspired by Altium, `KiCad-Db-Lib` creates one or more KiCad Symbol Libraries with atomic parts based on your database.  
Create and maintain a database for your electric components with

- symbol reference
- footprint reference
- value
- reference (R, L, C, etc.)
- description
- datasheet
- keywords
- and your custom fields (manufacturer, order codes etc.)

inside of `KiCad-Db-Lib`.

Created with Angular and Electron KiCad-Db-Lib can be used on Windows, Linux and MacOS.

![Screenshot](documentation/screenshot-parts.png 'Screenshot')

## Usage

On Windows, download the `electron-packager/kicad-db-lib-win32-x64` folder and run the `kicad-db-lib.exe`.
At first startup you have to go to the ![Settings][settings] settings and configure your custom fields and your paths.

### Settings

#### Fields:

Add or delete custom fields like manufacturer, order codes etc.

#### Paths

- Parts:

  Directory where your created parts are stored. E.g. `C:\kicad\parts`.

- Symbol:

  Directory where the KiCad symbols are stored. E.g. clone the `kicad-symbols` repository from https://github.com/KiCad/kicad-symbols to `C:\kicad\kicad-symbols`.

- Footprint:

  Directory where toe KiCad footprints are stored. On Windows, it is `C:\Program Files\KiCad\share\kicad\modules`.

- Output:

  Directory where your builded library files are stored. E.g. `C:\kicad\library`.

### Add, update, delete part

To add parts you have to click on the ![Add][add] button. Click on a part in the Parts-View to update or delete it.  
The application will build your library whenever a part is added, updated or deleted.  
You can reload the parts from your disk or rebuild the library at any time by using the corresponding button from the menu ![More vertical][more-vert]

After creating some parts with `KiCad-Db-Lib` you can use the library files from the `output` path in KiCad.

## Roadmap

- [x] Add sort functionality to parts data table
- [ ] Add filter functionality to parts data table
- [x] Function to duplicate part
- [x] Add pard id to created library
- [ ] Validate parts (unique value per library)
- [ ] Remove footprint list

  > `$FPLIST`  
  > `DIP?14*`  
  > `$ENDFPLIST`

  from the symbol template as well

- [ ] Configure electron packager for Linux and Mac OS
- [ ] Setup guide on first application usage

[settings]: https://material.io/tools/icons/static/icons/baseline-settings-20px.svg 'Settings Icon'
[add]: https://material.io/tools/icons/static/icons/baseline-add-24px.svg 'Add Icon'
[more-vert]: https://material.io/tools/icons/static/icons/baseline-more_vert-24px.svg 'More vert Icon'
