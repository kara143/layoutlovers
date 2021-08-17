using System;
using System.Linq;

namespace layoutlovers.Purchases
{
    public interface IPurchaseManager : IAppManagerBase<Purchase, Guid>
    {
        IQueryable<Purchase> GetAllByUserId(long userId);
    }
}
