using System.Threading.Tasks;
using Abp.Application.Services;
using layoutlovers.Configuration.Tenants.Dto;

namespace layoutlovers.Configuration.Tenants
{
    public interface ITenantSettingsAppService : IApplicationService
    {
        Task<TenantSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(TenantSettingsEditDto input);

        Task ClearLogo();

        Task ClearCustomCss();
    }
}
