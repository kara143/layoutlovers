using System;
using System.Threading.Tasks;

namespace layoutlovers.Products
{
    public interface IProductManager : IAppManagerBase<Product, Guid>
    {
        Task<Product> GetById(Guid id);
        Product GetByIdAndIncluding(Guid id);
    }
}
