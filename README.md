# KiCad-Db-Lib

- [KiCad-Db-Lib](#kicad-db-lib)
  - [About](#about)
  - [Usage](#usage)
    - [Settings](#settings)
      - [Paths](#paths)
      - [Fields:](#fields)
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

Because it is build with [Avalonia](http://avaloniaui.net/) KiCad-Db-Lib can be used on Windows, Linux and MacOS.

![Screenshot](documentation/screenshot-parts.png 'Screenshot')

## Usage

On Windows, download and unpack the `kicad-db-lib-win-x64.zip` from the [latest release](https://github.com/Projektanker/kicad-db-lib/releases/latest) and run `KiCadDbLib.exe`.

On Linux, download and unpack the `kicad-db-lib-linux-x64.zip` from the [latest release](https://github.com/Projektanker/kicad-db-lib/releases/latest) and run `KiCadDbLib`.

At first startup you have to go to the settings and configure your custom fields and your paths.

### Settings


#### Paths

- Parts:

  The parts folder is to store the parts created using kicad-db-lib. Every part you create is stored as a single JSON file. So it is possible to sync your parts across multiple devices by DropBox, OneDrive etc. E.g. `C:\kicad\parts`.

- Symbol:

  Directory where the KiCad symbols are stored. E.g. clone the `kicad-symbols` repository from https://gitlab.com/kicad/libraries/kicad-symbols to `C:\kicad\kicad-symbols`.

- Footprint:

  Directory where the KiCad footprints are stored. On Windows, it is `C:\Program Files\KiCad\share\kicad\modules`.

- Output:

  Directory where the final created library files for use in KiCad go. E.g. `C:\kicad\library`.

#### Fields:

Add or delete custom fields like manufacturer, order codes etc.

### Add, update, delete part

To add parts you have to click on the `+` button. Click on a part in the Parts-View to update or delete it.  
The application will build your library whenever a part is added, updated or deleted.  
You can reload the parts from your disk or rebuild the library at any time by using the corresponding button from the menu.

After creating some parts with `KiCad-Db-Lib` you can use the library files from the `output` path in KiCad.

## Roadmap

- [ ] Add filter functionality to parts data table
- [ ] Validate parts (unique value per library)
- [ ] Setup guide on first application usage
