using System;

namespace layoutlovers.ShoppingCarts
{
    public interface IShoppingCart
    {
        Guid ProductId { get; set; }
        long UserId { get; set; }
    }
}
