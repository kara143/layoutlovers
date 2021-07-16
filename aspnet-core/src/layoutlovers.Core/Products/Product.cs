using Abp.Domain.Entities.Auditing;
using layoutlovers.Categories;
using System;
using System.ComponentModel.DataAnnotations.Schema;

public enum Compatibility
{
    Sketch,
    AdobePhotoshop,
    AdobeXD,
    Figma
}

namespace layoutlovers.Products
{
    [Table("AppProducts")]
    public class Product: FullAuditedEntity<Guid>
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string DownloadUrl { get; set; }
        public string Description { get; set; }
        public Guid CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
