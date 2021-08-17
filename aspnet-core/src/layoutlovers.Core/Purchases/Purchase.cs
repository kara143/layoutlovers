using layoutlovers.MultiTenancy.Payments;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace layoutlovers.Purchases
{
    [Table("AppPurchase")]
    public class Purchase: UserProductFullAuditedEntity<Guid>, IPurchase
    {
        public string Status { get; set; }
        public RequestPayerStatus RequestPayerStatus { get; set; }
        public string ChargeId { get; set; }
        public string ReceiptUrl { get; set; }
        public string FailureMessage { get; set; }
    }
}
