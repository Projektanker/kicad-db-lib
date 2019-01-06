# KiCadDbLib

- [KiCadDbLib](#kicaddblib)
  - [About](#about)
  - [Next steps:](#next-steps)

## About
Inspired by Altium, KiCadDbLib creates one or KiCad Symbol Libraries based on your database.  
Create and maintain a database for your electric components with 
- symbol reference
- footprint reference
- value
- reference (R, L, C, etc.)
- description
- datasheet
- keywords
- and your custom fields (manufacturer, order codes etc.)

inside of KiCadDbLib.

Written in C# using .NET Core and [Avalonia UI](https://github.com/AvaloniaUI/Avalonia) KiCadDbLib can be used on Windows, Linux and MacOS.

## Next steps:
- Remove footprint list  
  > `$FPLIST`  
  > `DIP?14*`  
  > `$ENDFPLIST`  
  
  the symbol templeate as well

- Basic UI to add, modify and delete parts
- Load selected *.lib-files and *.pretty-folders into tree view, so that user can select symbol and footprint instead typing in manually