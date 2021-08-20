using Abp.Domain.Repositories;
using layoutlovers.LayoutProducts;
using layoutlovers.MultiTenancy.Payments;
using layoutlovers.PurchaseItems;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace layoutlovers.Purchases
{
    public class PurchaseManager: AppManagerBase<Purchase, Guid>, IPurchaseManager
    {
        public PurchaseManager(IRepository<Purchase, Guid> repository) : base(repository) { }

        public async Task<Purchase> InsertPurchaseAsync(Charge charge, List<LayoutProduct> products, long userId)
        {
            var purchaseToCreate = CreateBlankPurchase(charge, products, userId);

            var purchase = await InsertAsync(purchaseToCreate);

            return purchase;
        }

        private Purchase CreateBlankPurchase(Charge charge, List<LayoutProduct> products, long userId)
        {
            var purchaseItems = products.Select((product, index) => new PurchaseItem
            {
                LayoutProductId = product.Id,
                UserId = userId,
                Amount = product.Amount
            }).ToList();

            var totalAmount = products.Select(f => f.Amount).Sum();

            var purchase = new Purchase
            {
                UserId = userId,
                PurchaseItems = purchaseItems,
                ChargeId = charge.Id,
                Status = charge.Status,
                ReceiptUrl = charge.ReceiptUrl,
                TotalAmount = totalAmount,
                PaymentProvider = PaymentProvider.Stripe
            };

            if (charge.Status.Equals("succeeded", StringComparison.InvariantCultureIgnoreCase))
            {
                purchase.RequestPayerStatus = RequestPayerStatus.Successful;
            }
            else
            {
                purchase.RequestPayerStatus = RequestPayerStatus.Failed;
                purchase.FailureMessage = charge.FailureMessage;
            }
            return purchase;
        }
    }
}
