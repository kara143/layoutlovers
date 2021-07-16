using System.Threading.Tasks;
using Abp.Application.Services;
using layoutlovers.Sessions.Dto;

namespace layoutlovers.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();

        Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken();
    }
}
