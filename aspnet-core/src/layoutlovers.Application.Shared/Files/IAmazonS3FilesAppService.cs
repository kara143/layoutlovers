using Abp.Application.Services;
using layoutlovers.Amazon;
using layoutlovers.Files.Dto;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace layoutlovers.Files
{
    public interface IAmazonS3FilesAppService : IApplicationService
    {
        Task<IAmazonS3File> GetById(Guid id);
        Task Delete(Guid id);
        Task<S3FileDtoBase> UploadFileAndAddToProduct(IFormFile file, Guid productId);
    }
}
