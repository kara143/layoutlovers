using Abp.Domain.Repositories;
using layoutlovers.Extensions;
using layoutlovers.LayoutProducts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace layoutlovers.PurchaseItems
{
    public class PurchaseItemManager : AppManagerBase<PurchaseItem, Guid>, IPurchaseItemManager
    {
        public PurchaseItemManager(IRepository<PurchaseItem, Guid> repository) : base(repository)
        {
        }

        public IQueryable<PurchaseItem> GetAllByUserId(long userId)
        {
            return GetAll().Where(f => f.UserId == userId);
        }

        public IQueryable<LayoutProduct> GetAllPurchasedProductByUserId(long id)
        {
            return GetAllIncluding(f => f.LayoutProduct)
                .Select(f => f.LayoutProduct);
        }

        public async Task<bool> IsBought(Guid productId, long userId)
        {
            var product = await GetAll().FirstOrDefaultAsync(f => f.UserId == userId && f.LayoutProductId == productId);
            return product.IsNotNull();
        }
    }
}
