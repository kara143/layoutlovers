using layoutlovers.Files;
using layoutlovers.Products;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace layoutlovers.Amazon
{
    [Table("AppAmazonS3File")]
    public class AmazonS3File : FullAuditedEntityWithName<Guid>, IAmazonS3File
    {
        public FileType FileExtension { get; set; }
        public bool IsImage { get; set; }
        public Guid ProductId { get; set; }
        public virtual Product  Product { get; set; }
    }
}
