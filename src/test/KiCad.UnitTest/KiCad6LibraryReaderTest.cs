using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using KiCadDbLib.Models;
using KiCadDbLib.Services;
using KiCadDbLib.Services.KiCad.LibraryReader;
using Moq;
using Xunit;

namespace KiCad.UnitTest;

public static class KicadDownloader
{
    public static async Task DownloadSymbolFile(string libraryName)
    {
        var file = $"{libraryName}.kicad_sym";
        if (File.Exists(file))
        {
            File.Delete(file);
        }

        using var client = new HttpClient();
        var url = new Uri("https://gitlab.com/kicad/libraries/kicad-symbols/-/raw/master/" + file);
        var content = await client.GetByteArrayAsync(url);
        await File.WriteAllBytesAsync(file, content);
    }
}

public class KiCad6LibraryReaderTest
{
    [Fact]
    public async Task Reader_Should_Read_Symbols()
    {
        // Arrange
        await KicadDownloader.DownloadSymbolFile("Device");

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

        symbols.Should().Contain("Device:C");
        symbols.Should().Contain("Device:R");
        symbols.Should().Contain("Device:L");

        symbols.Should().NotContain("Device:C_0_0");
        symbols.Should().NotContain("Device:Filter_EMI_C", because: "it only extends Device:C_Feedthrough");
    }
}