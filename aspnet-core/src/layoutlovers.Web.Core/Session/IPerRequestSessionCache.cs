using System.Threading.Tasks;
using layoutlovers.Sessions.Dto;

namespace layoutlovers.Web.Session
{
    public interface IPerRequestSessionCache
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync();
    }
}
