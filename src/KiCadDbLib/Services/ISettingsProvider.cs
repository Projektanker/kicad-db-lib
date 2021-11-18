using System.Threading.Tasks;
using KiCadDbLib.Models;

namespace KiCadDbLib.Services
{
    public interface ISettingsProvider
    {
        string Location { get; }

        Task<Settings> GetSettingsAsync();

        Task SetSettingsAsync(Settings settings);
    }
}