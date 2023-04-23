using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KiCadDbLib.Models;

namespace KiCadDbLib.Services.KiCad.LibraryWriter
{
    public class KiCadLibraryWriterFactory : ILibraryWriterFactory
    {
        private readonly ISettingsProvider _settingsProvider;
        private KiCad6LibrarySymbolTemplateFactory? _kiCad6LibrarySymbolTemplateFactory;

        public KiCadLibraryWriterFactory(ISettingsProvider settingsProvider)
        {
            _settingsProvider = settingsProvider;
        }

        public async Task<ILibraryWriter> CreateWriterAsync(string outputDirectory, string libraryName)
        {
            var settings = await _settingsProvider.GetWorkspaceSettings()
                .ConfigureAwait(false);

            if (IsLegacy(settings))
            {
                return new LegacyKiCadLibraryWriter(settings.SymbolsPath, outputDirectory, libraryName);
            }
            else
            {
                _kiCad6LibrarySymbolTemplateFactory ??= new();
                return new KiCad6LibraryWriter(_kiCad6LibrarySymbolTemplateFactory, settings.SymbolsPath, outputDirectory, libraryName);
            }
        }

        private static bool IsLegacy(WorkspaceSettings settings)
        {
            return !Directory
                .EnumerateFiles(settings.SymbolsPath, $"*{FileExtensions.KicadSym}")
                .Any();
        }
    }
}