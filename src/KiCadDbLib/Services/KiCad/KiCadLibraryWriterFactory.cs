using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using KiCadDbLib.Models;

namespace KiCadDbLib.Services.KiCad
{

    public class KiCadLibraryWriterFactory : ILibraryWriterFactory
    {
        private ISettingsProvider _settingsProvider;

        public KiCadLibraryWriterFactory(ISettingsProvider settingsProvider)
        {
            _settingsProvider = settingsProvider;
        }

        public async Task<ILibraryWriter> CreateWriterAsync(string outputDirectory, string libraryName)
        {
            var settings = await _settingsProvider.GetSettingsAsync()
                .ConfigureAwait(false);

            return new KiCadLibraryWriter(settings.SymbolsPath, outputDirectory, libraryName);
        }
    }
}