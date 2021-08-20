using layoutlovers.MultiTenancy.Payments;
using System;

namespace layoutlovers.PurchaseItems.Dto.Base
{
    public class PurchaseItemBase : IPurchaseItem
    {
        public long UserId { get; set; }
        public Guid LayoutProductId { get; set; }
        public RequestPayerStatus RequestPayerStatus { get; set; }
        public decimal Amount { get; set; }
        public Guid PurchaseId { get; set; }
    }
}
