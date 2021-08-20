using Abp.UI;
using layoutlovers.Amazon;
using layoutlovers.DownloadAmazonS3Files;
using layoutlovers.Files;
using layoutlovers.Files.Dto;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace layoutlovers.AmazonS3Files
{
    public class AmazonS3FilesAppService : layoutloversAppServiceBase, IAmazonS3FilesAppService
    {
        private readonly IAmazonS3Manager _amazonS3Manager;
        private readonly AmazonS3Configuration _amazonS3Configuration;
        private readonly IDownloadAmazonS3FileManager _downloadAmazonS3FileManager;

        public AmazonS3FilesAppService(IAmazonS3Manager amazonS3Manager
            , AmazonS3Configuration amazonS3Configuration
            , IDownloadAmazonS3FileManager downloadAmazonS3FileManager
            )
        {
            _amazonS3Manager = amazonS3Manager;
            _amazonS3Configuration = amazonS3Configuration;
            _downloadAmazonS3FileManager = downloadAmazonS3FileManager;
        }

        public async Task<S3FileDtoBase> UploadFileAndAddToProduct(IFormFile file, Guid productId)
        {
            var entity = await _amazonS3Manager.UploadToS3AndInsert(file, productId);
            var dto = ObjectMapper.Map<S3FileDtoBase>(entity);
            return dto;
        }

        public IEnumerable<IAmazonS3File> GetPreviewImagesByProductId(Guid id)
        {
            var images = _amazonS3Manager.GetAllByProductId(id)
                .Where(f => f.IsImage)
                .ToList();

            var dtos = ObjectMapper.Map<IEnumerable<S3ImageDto>>(images);
            foreach (var item in dtos)
            {
                item.PreviewUrl = string.Format(_amazonS3Configuration.ThumbnailImages
                    , _amazonS3Configuration.BucketName
                    , _amazonS3Configuration.Region
                    , item.LayoutProductId
                    , item.Name);
            }

            return dtos;
        }

        /// <summary>
        /// We will save the download details and return the download url!
        /// </summary>
        /// <param name="id">
        /// The Шd of the file to be downloaded.
        /// </param>
        /// <returns></returns>
        public async Task<S3FileDto> GetDownloadUrlByFileId(Guid id)
        {
            var tenant = await GetCurrentTenantAsync();

            if (!tenant.EditionId.HasValue)
            {
                throw new UserFriendlyException($"Edition not found");
            }

            var editionId = (int)tenant.EditionId;

            var user = await GetCurrentUserAsync();

            await _downloadAmazonS3FileManager.SaveAsync(editionId, id, user);

            var file = await _amazonS3Manager.FirstOrDefaultAsync(f => f.Id == id && !f.IsImage);

            var fileDto = ObjectMapper.Map<S3FileDto>(file);
            fileDto.DownloadUrl = string.Format(_amazonS3Configuration.FileTypes
                    , _amazonS3Configuration.BucketName
                    , _amazonS3Configuration.Region
                    , file.LayoutProductId
                    , file.Name);

            return fileDto;
        }

        public async Task Delete(Guid id)
        {
            await _amazonS3Manager.DeleteFromS3AndDbById(id);
        }

        public async Task<IAmazonS3File> GetById(Guid id)
        {
            var s3Filt = await _amazonS3Manager.GetById(id);
            var dto = MapS3FileToDto(s3Filt);
            return dto;
        }

        private IAmazonS3File MapS3FileToDto(IAmazonS3File file)
        {
            if (file.IsImage)
            {
                var result = ObjectMapper.Map<S3ImageDto>(file);
                result.PreviewUrl = string.Format(_amazonS3Configuration.ThumbnailImages
                    , _amazonS3Configuration.BucketName
                    , _amazonS3Configuration.Region
                    , file.LayoutProductId
                    , file.Name);
                return result;
            }
            else
            {
                var result = ObjectMapper.Map<S3FileDto>(file);
                result.DownloadUrl = string.Format(_amazonS3Configuration.FileTypes
                    , _amazonS3Configuration.BucketName
                    , _amazonS3Configuration.Region
                    , file.LayoutProductId
                    , file.Name);
                return result;
            }
        }
    }
}
