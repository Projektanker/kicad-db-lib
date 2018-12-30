using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Projektanker.KiCad.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async void TestMethod1()
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
                    Reference = "R",
                    Value = "10u_16V",
                    Footprint = "Capacitor_SMD:C_0805_2012Metric",
                    Library = "C_0805",
                    Description = "Capacitor 10uF 0805 16V",
                    Keywords = "Capacitor Capacitor 10u 0805",
                    Symbol = $"{lib}:C"
                }
            };

            //await KiCadLibraryBuilder.BuildAsync(parts, output);
            Assert.IsTrue(true);
        }
    }
}