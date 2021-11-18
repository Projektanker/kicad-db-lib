using System.Threading.Tasks;
using KiCadDbLib.Models;

namespace KiCadDbLib.Services
{
    public interface IPartRepository
    {
        Task AddOrUpdateAsync(Part part);
        Task DeleteAsync(string id);
        Task<string[]> GetLibrariesAsync();
        Task<Part> GetPartAsync(string id);
        Task<Part[]> GetPartsAsync();
    }
}