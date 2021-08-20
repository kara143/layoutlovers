using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace layoutlovers.ShoppingCarts
{
    public class ShoppingCartManager: AppManagerBase<ShoppingCart, Guid>, IShoppingCartManager
    {
        public ShoppingCartManager(IRepository<ShoppingCart, Guid> repository) : base(repository) { }

        public IEnumerable<ShoppingCart> GetShoppingCartItemsByUserId(long id)
        {
            var shoppingCartItems = _repository.GetAllIncluding(f => f.LayoutProduct)
                .Where(f => f.UserId == id)
                .ToList();

            return shoppingCartItems;
        }
    }
}
