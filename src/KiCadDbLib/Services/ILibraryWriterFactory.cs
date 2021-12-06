using System.Threading.Tasks;

namespace KiCadDbLib.Services
{
    public interface ILibraryWriterFactory
    {
        Task<ILibraryWriter> CreateWriterAsync(string outputDirectory, string libraryName);
    }
}