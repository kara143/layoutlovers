using Abp.Domain.Entities.Auditing;
using layoutlovers.MultiTenancy.Payments;
using layoutlovers.PurchaseItems;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace layoutlovers.Purchases
{
    [Table("AppPurchases")]
    public class Purchase: FullAuditedEntity<Guid>, IPurchase
    {
        public long UserId { get; set; }
        public string Status { get; set; }
        public RequestPayerStatus RequestPayerStatus { get; set; }
        public PaymentProvider PaymentProvider { get; set; }
        public string ChargeId { get; set; }
        public string ReceiptUrl { get; set; }
        public string FailureMessage { get; set; }
        public decimal TotalAmount { get; set; }
        public virtual ICollection<PurchaseItem> PurchaseItems { set; get; }
    }
}
