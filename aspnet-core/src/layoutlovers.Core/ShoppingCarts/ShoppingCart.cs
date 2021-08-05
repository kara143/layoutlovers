using Abp.Domain.Entities.Auditing;
using layoutlovers.Authorization.Users;
using layoutlovers.Products;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace layoutlovers.ShoppingCarts
{
    [Table("AppShoppingCarts")]
    public class ShoppingCart : FullAuditedEntity<Guid>, IShoppingCart
    {
        public Guid ProductId { get; set; }
        public virtual Product Product{get; set;}
        public long UserId { get; set; }
        public virtual User User { get; set; }
    }
}
