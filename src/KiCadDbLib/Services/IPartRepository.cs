using System.Threading.Tasks;
using KiCadDbLib.Models;

namespace KiCadDbLib.Services
{
    public interface IPartRepository
    {
        Task AddOrUpdateAsync(Part part);

        Task DeleteAsync(int id);

        Task<string[]> GetLibrariesAsync();

        Task<Part> GetPartAsync(int id);

        Task<Part[]> GetPartsAsync();
    }
}