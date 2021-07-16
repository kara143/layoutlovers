using System.Threading.Tasks;
using Abp.Application.Services;
using layoutlovers.Editions.Dto;
using layoutlovers.MultiTenancy.Dto;

namespace layoutlovers.MultiTenancy
{
    public interface ITenantRegistrationAppService: IApplicationService
    {
        Task<RegisterTenantOutput> RegisterTenant(RegisterTenantInput input);

        Task<EditionsSelectOutput> GetEditionsForSelect();

        Task<EditionSelectDto> GetEdition(int editionId);
    }
}