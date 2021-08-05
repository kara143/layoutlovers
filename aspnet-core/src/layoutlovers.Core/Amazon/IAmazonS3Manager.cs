using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace layoutlovers.Amazon
{
    public interface IAmazonS3Manager: IAppManagerBase<AmazonS3File, Guid>
    {
        Task DeleteFromS3(AmazonS3File file);
        Task<AmazonS3File> UploadToS3AndInsert(IFormFile file, Guid productId);
        Task UploadToS3(IFormFile file, string productId);
        Task DeleteFromS3AndDbById(Guid id);
        IEnumerable<IAmazonS3File> GetAllByProductId(Guid id);
        Task<IAmazonS3File> GetById(Guid id);
    }
}
