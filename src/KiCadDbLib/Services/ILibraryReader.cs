using System.Threading.Tasks;

namespace KiCadDbLib.Services
{
    public interface ILibraryReader
    {
        Task<string[]> GetFootprintsAsync();
        Task<string[]> GetSymbolsAsync();
    }
}