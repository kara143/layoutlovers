using System;

namespace layoutlovers.ShoppingCarts.Dto.Base
{
    public class ShoppingCartBase : IShoppingCart
    {
        public Guid LayoutProductId { get; set; }
        public long UserId { get; set; }
    }
}
