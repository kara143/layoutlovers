using layoutlovers.Files;
using System;

namespace layoutlovers.Amazon
{
    public interface IAmazonS3File: INameBase
    {
        FileType FileExtension { get; set; }
        bool IsImage { get; set; }
        Guid LayoutProductId { get; set; }
    }
}
