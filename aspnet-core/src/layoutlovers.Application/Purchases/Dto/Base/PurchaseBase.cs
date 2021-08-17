using layoutlovers.MultiTenancy.Payments;
using System;

namespace layoutlovers.Purchases.Dto.Base
{
    public class PurchaseBase : IPurchase
    {
        public long UserId { get; set; }
        public Guid LayoutProductId { get; set; }
        public string Status { get; set; }
        public string ChargeId { get; set; }
        public string ReceiptUrl { get; set; }
        public string FailureMessage { get; set; }
        public RequestPayerStatus RequestPayerStatus { get; set; }
    }
}
