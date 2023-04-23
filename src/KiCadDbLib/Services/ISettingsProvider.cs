using System.Threading.Tasks;
using KiCadDbLib.Models;

namespace KiCadDbLib.Services
{
    public interface ISettingsProvider
    {
        string Location { get; }

        Task<AppSettings> GetAppSettings();

        Task UpdateAppSettings(AppSettings appSettings);

        Task<WorkspaceSettings> GetWorkspaceSettings();

        Task UpdateWorkspaceSettings(WorkspaceSettings settings);
    }
}