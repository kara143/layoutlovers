using Abp.Domain.Repositories;
using layoutlovers.FilterTags;
using System;
using System.Collections.Generic;
using System.Linq;

namespace layoutlovers.ProductFilterTags
{
    public class ProductFilterTagManager : AppManagerBase<ProductFilterTag, Guid>, IProductFilterTagManager
    {
        public ProductFilterTagManager(IRepository<ProductFilterTag, Guid> repository) : base(repository)
        {
        }

        public IEnumerable<FilterTag> GetFilterTagByProductId(Guid productId)
        {
            return _repository.GetAllIncluding(f => f.FilterTag)
                .Where(f => f.LayoutProductId == productId)
                .Select(f => f.FilterTag)
                .ToArray();
        }

        public IEnumerable<FilterTag> GetFilterTagByCategoryId(Guid categoryId)
        {
            return _repository.GetAllIncluding(f => f.FilterTag, f => f.LayoutProduct)
                .Where(f => f.LayoutProductId == f.LayoutProduct.Id)
                .Where(f => f.LayoutProduct.CategoryId == categoryId)
                .Where(f => !f.IsDeleted)
                .Select(f => f.FilterTag)
                .ToArray();
        }
        
        public IEnumerable<FilterTag> GetFilterTagForFeatured()
        {
            return _repository.GetAllIncluding(f => f.FilterTag, f => f.LayoutProduct)
                .Where(f => f.LayoutProductId == f.LayoutProduct.Id)
                .Where(f => f.LayoutProduct.IsFeatured)
                .Where(f => !f.IsDeleted)
                .Select(f => f.FilterTag)
                .ToArray();
        }
    }
}
