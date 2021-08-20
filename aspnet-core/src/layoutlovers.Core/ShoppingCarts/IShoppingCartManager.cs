using System;
using System.Collections.Generic;

namespace layoutlovers.ShoppingCarts
{
    public interface IShoppingCartManager : IAppManagerBase<ShoppingCart, Guid>
    {
        IEnumerable<ShoppingCart> GetShoppingCartItemsByUserId(long id);
    }
}