using System;

namespace layoutlovers.DownloadAmazonS3Files
{
    public interface IDownloadAmazonS3File
    {
        long UserId { get; set; }
        Guid AmazonS3FileId { get; set; }
    }
}
