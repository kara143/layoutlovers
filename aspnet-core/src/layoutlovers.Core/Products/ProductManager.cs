using Abp.Domain.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace layoutlovers.Products
{
    public class ProductManager : AppManagerBase<Product, Guid>, IProductManager
    {
        public ProductManager(IRepository<Product, Guid> repository) : base(repository)
        {
        }

        public Product GetByIdAndIncluding(Guid id)
        {
            return _repository.GetAllIncluding(f => f.Category
            , f => f.AmazonS3Files
            , f => f.ProductFilterTags)
            .FirstOrDefault(f => f.Id == id);
        }

        public Task<Product> GetById(Guid id)
        {
            return _repository.GetAsync(id);
        }
    }
}
