using layoutlovers.MultiTenancy.Payments;

namespace layoutlovers.Purchases.Dto.Base
{
    public class PurchaseBase : IPurchase
    {
        public long UserId { get; set; }
        public string Status { get; set; }
        public RequestPayerStatus RequestPayerStatus { get; set; }
        public string ChargeId { get; set; }
        public string ReceiptUrl { get; set; }
        public string FailureMessage { get; set; }
        public PaymentProvider PaymentProvider { get; set; }
    }
}
