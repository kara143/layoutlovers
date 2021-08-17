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
    }
}
