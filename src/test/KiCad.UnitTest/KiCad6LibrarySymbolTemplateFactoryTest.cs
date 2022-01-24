using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using KiCadDbLib.Services.KiCad;
using KiCadDbLib.Services.KiCad.LibraryWriter;
using Xunit;

namespace KiCad.UnitTest;

public class KiCad6LibrarySymbolTemplateFactoryTest
{
    [Fact]
    public async Task Factory_Should_Read_Symbol()
    {
        // Arrange
        await KicadDownloader.DownloadSymbolFile("Device");

        var symbolInfo = new LibraryItemInfo("Device", "R");
        var factory = new KiCad6LibrarySymbolTemplateFactory();

        // Act
        var symbol = await factory.GetSymbolTemplateAsync(Directory.GetCurrentDirectory(), symbolInfo);

        // Assert
        symbol.Should().NotBeNull();
        symbol.Name.Should().Be("symbol");
        symbol.Childs[0].Name.Should().Be("R");
    }
}