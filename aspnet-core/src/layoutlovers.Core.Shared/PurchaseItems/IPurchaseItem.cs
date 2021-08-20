using System;

namespace layoutlovers.PurchaseItems
{
    public interface IPurchaseItem : IUserProductBase
    {
        decimal Amount { get; set; }
        Guid PurchaseId { get; set; }
    }
}
