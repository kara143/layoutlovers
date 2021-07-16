using System.Threading.Tasks;
using layoutlovers.Authorization.Users;

namespace layoutlovers.WebHooks
{
    public interface IAppWebhookPublisher
    {
        Task PublishTestWebhook();
    }
}
