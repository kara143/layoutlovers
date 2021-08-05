using layoutlovers.ProductFilterTags;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace layoutlovers.FilterTags
{
    [Table("AppFilterTags")]
    public class FilterTag : FullAuditedEntityWithName<Guid>, IFilterTag
    {
        public virtual ICollection<ProductFilterTag> ProductFilterTags { get; set; }
    }
}
