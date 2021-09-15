using layoutlovers.LayoutProducts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace layoutlovers.PurchaseItems
{
    public interface IPurchaseItemManager : IAppManagerBase<PurchaseItem, Guid>
    {
        IQueryable<PurchaseItem> GetAllByUserId(long userId);
        IQueryable<LayoutProduct> GetAllPurchasedProductByUserId(long id);
        IQueryable<LayoutProduct> GetPurchasedProductsByUserId(long id);
        Task<bool> IsBought(Guid productId, long userId);
    }
}
