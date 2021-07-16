using System.Threading.Tasks;
using Abp.Application.Services;
using layoutlovers.MultiTenancy.Payments.PayPal.Dto;

namespace layoutlovers.MultiTenancy.Payments.PayPal
{
    public interface IPayPalPaymentAppService : IApplicationService
    {
        Task ConfirmPayment(long paymentId, string paypalOrderId);

        PayPalConfigurationDto GetConfiguration();
    }
}
