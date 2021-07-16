using System.Threading.Tasks;
using Abp.Webhooks;

namespace layoutlovers.WebHooks
{
    public interface IWebhookEventAppService
    {
        Task<WebhookEvent> Get(string id);
    }
}
