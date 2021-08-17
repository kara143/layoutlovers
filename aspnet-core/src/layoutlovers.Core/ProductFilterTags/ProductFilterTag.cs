using Abp.Domain.Entities.Auditing;
using layoutlovers.FilterTags;
using layoutlovers.LayoutProducts;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace layoutlovers.ProductFilterTags
{
    [Table("AppProductFilterTags")]
    public class ProductFilterTag: FullAuditedEntity<Guid>, IProductFilterTag
    {
        public Guid LayoutProductId { get; set; }
        public virtual LayoutProduct LayoutProduct { get; set; }
        public Guid FilterTagId { get; set; }
        public virtual FilterTag FilterTag { get; set; }
    }
}
