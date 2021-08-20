using layoutlovers.MultiTenancy.Payments;

namespace layoutlovers.Purchases
{
    public interface IPurchase
    {
        long UserId { get; set; }
        string Status { get; set; }
        RequestPayerStatus RequestPayerStatus { get; set; }
        PaymentProvider PaymentProvider { get; set; }
        string ChargeId { get; set; }
        string ReceiptUrl { get; set; }
        string FailureMessage { get; set; }
    }
}
