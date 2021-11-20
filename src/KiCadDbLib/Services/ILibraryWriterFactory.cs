using System.Linq;
using System.Threading.Tasks;
using KiCadDbLib.Models;
using KiCadDbLib.Services.KiCad;

namespace KiCadDbLib.Services
{

    public interface ILibraryWriterFactory
    {
        Task<ILibraryWriter> CreateWriterAsync(string outputDirectory, string libraryName);
    }
}