using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using KiCadDbLib.Models;
using KiCadDbLib.Services;
using KiCadDbLib.Services.KiCad;
using Moq;
using Xunit;

namespace KiCad.UnitTest
{
    public class KiCad6LibraryReaderTest
    {
        [Fact]
        public async Task Reader_Should_Read_Symbols()
        {
            // Arrange
            var settings = new Settings { SymbolsPath = Directory.GetCurrentDirectory() };
            var settingsProvider = new Mock<ISettingsProvider>();
            settingsProvider.Setup(x => x.GetSettingsAsync()).ReturnsAsync(settings);

            var reader = new KiCad6LibraryReader(settingsProvider.Object);

            // Act
            var symbols = await reader.GetSymbolsAsync();

            // Assert
            symbols.Should().HaveCountGreaterThan(0);
            foreach (var symbol in symbols)
            {
                symbol.Should().StartWith("Device:");
            }
        }
    }
}