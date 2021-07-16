using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace layoutlovers.Categories
{
    public class CategoryManager: layoutloversServiceBase, ICategoryManager
    {
        private readonly IRepository<Category, Guid> _categoryRepository;
        public CategoryManager(IRepository<Category, Guid> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await _categoryRepository.GetAllListAsync();
        }
    }
}
