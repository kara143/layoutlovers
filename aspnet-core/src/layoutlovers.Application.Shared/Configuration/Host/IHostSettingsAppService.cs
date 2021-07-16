using System.Threading.Tasks;
using Abp.Application.Services;
using layoutlovers.Configuration.Host.Dto;

namespace layoutlovers.Configuration.Host
{
    public interface IHostSettingsAppService : IApplicationService
    {
        Task<HostSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(HostSettingsEditDto input);

        Task SendTestEmail(SendTestEmailInput input);
    }
}
