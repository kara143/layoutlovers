using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace layoutlovers.ShoppingCarts
{
    [Table("AppShoppingCarts")]
    public class ShoppingCart : UserProductFullAuditedEntity<Guid>, IShoppingCart
    {
    }
}
