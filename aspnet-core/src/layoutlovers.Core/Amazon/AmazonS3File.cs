using layoutlovers.DownloadAmazonS3Files;
using layoutlovers.Files;
using layoutlovers.LayoutProducts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace layoutlovers.Amazon
{
    [Table("AppAmazonS3File")]
    public class AmazonS3File : FullAuditedEntityWithName<Guid>, IAmazonS3File
    {
        public FileType FileExtension { get; set; }
        public bool IsImage { get; set; }
        public int CountDownloads { get; set; }
        public Guid LayoutProductId { get; set; }
        public virtual LayoutProduct  LayoutProduct { get; set; }
        public virtual ICollection<DownloadAmazonS3File> DownloadAmazonS3Files { get; set; }
    }
}
