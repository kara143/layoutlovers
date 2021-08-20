using layoutlovers.LayoutProducts;
using Stripe;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace layoutlovers.Purchases
{
    public interface IPurchaseManager : IAppManagerBase<Purchase, Guid>
    {
        Task<Purchase> InsertPurchaseAsync(Charge charge, List<LayoutProduct> products, long userId);
    }
}