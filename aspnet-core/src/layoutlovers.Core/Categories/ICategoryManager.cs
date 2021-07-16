using Abp.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace layoutlovers.Categories
{
    public interface ICategoryManager: IDomainService
    {
        Task<IEnumerable<Category>> GetCategories();
    }
}
