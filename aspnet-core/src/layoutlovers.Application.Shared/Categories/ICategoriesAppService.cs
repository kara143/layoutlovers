using Abp.Application.Services;
using layoutlovers.Categories.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace layoutlovers.Categories
{
    public interface ICategoriesAppService : IApplicationService
    {
        Task<IEnumerable<CategoryDto>> GetAll();
    }
}
