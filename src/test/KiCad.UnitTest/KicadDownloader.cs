using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace KiCad.UnitTest;

public static class KicadDownloader
{
    private static readonly SemaphoreSlim _semaphore = new(1);

    public static async Task DownloadSymbolFile(string libraryName)
    {
        await _semaphore.WaitAsync();
        try
        {
            var file = $"{libraryName}.kicad_sym";
            if (File.Exists(file))
            {
                return;
            }

            using var client = new HttpClient();
            var url = new Uri("https://gitlab.com/kicad/libraries/kicad-symbols/-/raw/master/" + file);
            var content = await client.GetByteArrayAsync(url);
            await File.WriteAllBytesAsync(file, content);
            await Task.Delay(1000);
        }
        finally
        {
            _semaphore.Release();
        }
    }
}
