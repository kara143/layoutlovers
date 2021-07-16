using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using layoutlovers.Common.Dto;
using layoutlovers.Editions.Dto;

namespace layoutlovers.Common
{
    public interface ICommonLookupAppService : IApplicationService
    {
        Task<ListResultDto<SubscribableEditionComboboxItemDto>> GetEditionsForCombobox(bool onlyFreeItems = false);

        Task<PagedResultDto<NameValueDto>> FindUsers(FindUsersInput input);

        GetDefaultEditionNameOutput GetDefaultEditionName();
    }
}