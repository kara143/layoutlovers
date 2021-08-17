using layoutlovers.MultiTenancy.Payments;

namespace layoutlovers.Purchases
{
    public interface IPurchase : IUserProductBase
    {
        string Status { get; set; }
        RequestPayerStatus RequestPayerStatus { get; set; }
        string ChargeId { get; set; }
        string ReceiptUrl { get; set; }
        string FailureMessage { get; set; }
    }
}
