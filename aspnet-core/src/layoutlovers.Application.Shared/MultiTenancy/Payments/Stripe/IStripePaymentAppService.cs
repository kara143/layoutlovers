﻿using System.Threading.Tasks;
using Abp.Application.Services;
using layoutlovers.MultiTenancy.Payments.Dto;
using layoutlovers.MultiTenancy.Payments.Stripe.Dto;

namespace layoutlovers.MultiTenancy.Payments.Stripe
{
    public interface IStripePaymentAppService : IApplicationService
    {
        Task ConfirmPayment(StripeConfirmPaymentInput input);

        StripeConfigurationDto GetConfiguration();

        Task<SubscriptionPaymentDto> GetPaymentAsync(StripeGetPaymentInput input);

        Task<string> CreatePaymentSession(StripeCreatePaymentSessionInput input);
    }
}