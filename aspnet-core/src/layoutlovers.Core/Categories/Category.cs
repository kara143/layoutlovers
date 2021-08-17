using layoutlovers.LayoutProducts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace layoutlovers.Categories
{
    [Table("AppCategories")]
    public class Category: FullAuditedEntityWithName<Guid>, ICategory
    {
        public virtual ICollection<LayoutProduct> Products { set; get; }
    }
}
