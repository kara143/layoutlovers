using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace layoutlovers.LayoutProducts
{
    public class LayoutProductManager : AppManagerBase<LayoutProduct, Guid>, ILayoutProductManager
    {
        public LayoutProductManager(IRepository<LayoutProduct, Guid> repository) : base(repository)
        {
        }

        public LayoutProduct GetByIdAndIncluding(Guid id)
        {
            return _repository.GetAllIncluding(f => f.Category
            , f => f.AmazonS3Files
            , f => f.ProductFilterTags)
            .FirstOrDefault(f => f.Id == id);
        }

        public Task<LayoutProduct> GetById(Guid id)
        {
            return _repository.GetAsync(id);
        }
    }
}
