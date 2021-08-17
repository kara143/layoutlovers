using Abp.Application.Services.Dto;
using layoutlovers.Amazon;
using System;

namespace layoutlovers.Files.Dto
{
    public class S3FileDtoBase: EntityDto<Guid>, IAmazonS3File
    {
        public string Name { get; set; }
        public FileType FileExtension { get; set; }
        public Guid LayoutProductId { get; set; }
        public bool IsImage { get; set; }
    }
}
