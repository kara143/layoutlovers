using Abp.Authorization;
using layoutlovers.Categories.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace layoutlovers.Categories
{
    [AbpAuthorize]
    public class CategoriesAppService: layoutloversAppServiceBase, ICategoriesAppService
    {
        private readonly ICategoryManager _categoryManager;
        public CategoriesAppService(ICategoryManager categoryManager)
        {
            _categoryManager = categoryManager;
        }

        public async Task<IEnumerable<CategoryDto>> GetAll()
        {
            var categories = await _categoryManager.GetCategories();
            var categoryDtos = ObjectMapper.Map<IEnumerable<CategoryDto>>(categories);
            return categoryDtos;
        }
    }
}
