using Abp.Domain.Entities.Auditing;
using layoutlovers.Amazon;
using layoutlovers.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace layoutlovers.DownloadAmazonS3Files
{
    [Table("AppDownloadAmazonS3File")]
    public class DownloadAmazonS3File : FullAuditedEntity<Guid>, IDownloadAmazonS3File
    {
        public long UserId { get; set; }
        public virtual User User { get; set; }
        public Guid AmazonS3FileId { get; set; }
        public virtual AmazonS3File AmazonS3File { get; set; }
    }
}

