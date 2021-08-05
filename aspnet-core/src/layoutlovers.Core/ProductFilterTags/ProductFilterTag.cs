using Abp.Domain.Entities.Auditing;
using layoutlovers.FilterTags;
using layoutlovers.Products;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace layoutlovers.ProductFilterTags
{
    [Table("AppProductFilterTags")]
    public class ProductFilterTag: FullAuditedEntity<Guid>, IProductFilterTag
    {
        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; }
        public Guid FilterTagId { get; set; }
        public virtual FilterTag FilterTag { get; set; }
    }
}
