using Abp.Domain.Repositories;
using System;
using System.Linq;

namespace layoutlovers.Purchases
{
    public class PurchaseManager: AppManagerBase<Purchase, Guid>, IPurchaseManager
    {
        public PurchaseManager(IRepository<Purchase, Guid> repository): base(repository)
        {
        }

        public IQueryable<Purchase> GetAllByUserId(long userId)
        {
            return GetAll().Where(f => f.UserId == userId);
        }
    }
}
