using System.Threading.Tasks;
using Abp.Application.Services;

namespace layoutlovers.MultiTenancy
{
    public interface ISubscriptionAppService : IApplicationService
    {
        Task DisableRecurringPayments();

        Task EnableRecurringPayments();
    }
}
