using layoutlovers.Purchases;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace layoutlovers.PurchaseItems
{
    [Table("AppPurchaseItems")]
    public class PurchaseItem: UserProductFullAuditedEntity<Guid>, IPurchaseItem
    {
        public decimal Amount { get; set; }
        public Guid PurchaseId { get; set; }
        public virtual Purchase Purchase { get; set; }
    }
}
