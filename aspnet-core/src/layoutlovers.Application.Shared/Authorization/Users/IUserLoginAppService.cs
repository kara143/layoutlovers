using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using layoutlovers.Authorization.Users.Dto;

namespace layoutlovers.Authorization.Users
{
    public interface IUserLoginAppService : IApplicationService
    {
        Task<ListResultDto<UserLoginAttemptDto>> GetRecentUserLoginAttempts();
    }
}
