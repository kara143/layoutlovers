using Abp.Application.Services;
using Abp.Application.Services.Dto;
using layoutlovers.Authorization.Permissions.Dto;

namespace layoutlovers.Authorization.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
    }
}
