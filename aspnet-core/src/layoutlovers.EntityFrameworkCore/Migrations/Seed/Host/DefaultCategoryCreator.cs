using layoutlovers.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace layoutlovers.Migrations.Seed.Host
{
    class DefaultCategoryCreator
    {
        private readonly layoutloversDbContext _context;
        public DefaultCategoryCreator(layoutloversDbContext context)
        {
            _context = context;
        }
        public void Create()
        {
            CreateCategories();
        }

        private void CreateCategories()
        {
            var defaultCategories = _context.Categories.IgnoreQueryFilters().ToList();
            if (defaultCategories.Count == 0)
            {
                defaultCategories.Add(new Categories.Category {
                    Name = "Featured"
                });
                defaultCategories.Add(new Categories.Category
                {
                    Name = "Websites"
                });
                defaultCategories.Add(new Categories.Category
                {
                    Name = "Mockups "
                });
                defaultCategories.Add(new Categories.Category
                {
                    Name = "Mobile"
                });
                defaultCategories.Add(new Categories.Category
                {
                    Name = "Dashboards"
                });
                defaultCategories.Add(new Categories.Category
                {
                    Name = "Components/UI Kits"
                });
                defaultCategories.Add(new Categories.Category
                {
                    Name = "Social Media"
                });
                defaultCategories.Add(new Categories.Category
                {
                    Name = "Packages"
                });
                defaultCategories.Add(new Categories.Category
                {
                    Name = "Palettes"
                });

                _context.Categories.AddRange(defaultCategories);
                _context.SaveChanges();

                /* Add desired features to the standard edition, if wanted... */
            }
        }
    }
}
