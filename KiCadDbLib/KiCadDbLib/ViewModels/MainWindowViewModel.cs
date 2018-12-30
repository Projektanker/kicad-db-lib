using Projektanker.KiCad;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace KiCadDbLib.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Greeting => "Hello World!";

        public async void Test()
        {
            string lib = @"C:\WORKSPACE\GitHub\KiCadDbLib\testlib\Device.lib";
            string output = @"C:\WORKSPACE\GitHub\KiCadDbLib\testlib\output";

            List<KiCadPart> parts = new List<KiCadPart>
            {
                new KiCadPart()
                {
                    Reference = "R",
                    Value = "1K",
                    Footprint = "Resistor_SMD:R_0603_1608Metric",
                    Library = "R_0603",
                    Description = "Resistor 1K 0603 75V",
                    Keywords = "Res Resistor 1K 0603",
                    Symbol = $"{lib}:R"
                },

                new KiCadPart()
                {
                    Reference = "R",
                    Value = "10K",
                    Footprint = "Resistor_SMD:R_0603_1608Metric",
                    Library = "R_0603",
                    Description = "Resistor 10K 0603 75V",
                    Keywords = "Res Resistor 10K 0603",
                    Symbol = $"{lib}:R"
                },

                new KiCadPart()
                {
                    Reference = "R",
                    Value = "1K",
                    Footprint = "Resistor_SMD:R_0805_2012Metric",
                    Library = "R_0805",
                    Description = "Resistor 1K 0603 150V",
                    Keywords = "Res Resistor 1K 0603",
                    Symbol = $"{lib}:R"
                },

                new KiCadPart()
                {
                    Reference = "R",
                    Value = "10K",
                    Footprint = "Resistor_SMD:R_0805_2012Metric",
                    Library = "R_0805",
                    Description = "Resistor 10K 0805 150V",
                    Keywords = "Res Resistor 10K 0805",
                    Symbol = $"{lib}:R"
                },

                new KiCadPart()
                {
                    Reference = "C",
                    Value = "100n_50V",
                    Footprint = "Capacitor_SMD:C_0603_1608Metric",
                    Library = "C_0603",
                    Description = "Capacitor 100nF 50V 0603",
                    Keywords = "Cap Capacitor 100n 0603",
                    Symbol = $"{lib}:C"
                },

                new KiCadPart()
                {
                    Reference = "C",
                    Value = "10u_16V",
                    Footprint = "Capacitor_SMD:C_0805_2012Metric",
                    Library = "C_0805",
                    Description = "Capacitor 10uF 0805 16V",
                    Keywords = "Capacitor Capacitor 10u 0805",
                    Symbol = $"{lib}:C"
                }
            };

            KiCadLibraryBuilder builder = new KiCadLibraryBuilder();

            Stopwatch stw = new Stopwatch();
            stw.Start();
            await builder.BuildAsync(parts, output);
            stw.Stop();
            Console.WriteLine($"Milliseconds: {stw.ElapsedMilliseconds}");
            Console.WriteLine($"Ticks: {stw.ElapsedTicks}");

            KiCadLibraryReader reader = new KiCadLibraryReader();
            var symbols = await reader.GetSymbolsAsync(lib);
            Console.WriteLine($"Symbols Count: {symbols.Count}");
            int length = symbols.Count.ToString().Length;
            for (int i = 0; i < symbols.Count; i++)
            {
                Console.WriteLine($"{i,4}/{symbols.Count} {symbols[i]}");
            }
        }
    }
}