using System;
using System.Collections.Generic;
using System.Text;

namespace KiCadDbLib.Services.KiCad
{
    static class FileExtensions
    {
        public static string Dcm { get; } = ".dcm";
        public static string Lib { get; } = ".lib";
        public static string Pretty { get; } = ".pretty";
        public static string KicadMod { get; } = ".kicad_mod";
    }
}
