using layoutlovers.Amazon;
using layoutlovers.Files;
using layoutlovers.Files.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        private string _thumbnailImages = "https://{0}.s3.{1}.amazonaws.com/{2}/Thumbnail images/{3}";
        private string _fileTypes = "https://{0}.s3.{1}.amazonaws.com/{2}/File types/{3}";

        public AmazonS3FilesAppService(IAmazonS3Manager amazonS3Manager
            , AmazonS3Configuration amazonS3Configuration)
        {
            _amazonS3Manager = amazonS3Manager;
            _amazonS3Configuration = amazonS3Configuration;

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
                item.PreviewUrl = string.Format(_thumbnailImages
                    , _amazonS3Configuration.BucketName
                    , _amazonS3Configuration.Region
                    , item.ProductId
                    , item.Name);
            }

            return dtos;
        }

        public IEnumerable<IAmazonS3File> GetFilesToDownloadByProductId(Guid id)
        {
            var images = _amazonS3Manager.GetAll()
                .Where(f => f.ProductId == id)
                .Where(f => !f.IsImage)
                .ToList();

            var dtos = ObjectMapper.Map<IEnumerable<S3ImageDto>>(images);
            foreach (var item in dtos)
            {
                item.PreviewUrl = string.Format(_fileTypes
                    , _amazonS3Configuration.BucketName
                    , _amazonS3Configuration.Region
                    , item.ProductId
                    , item.Name);
            }

            return dtos;
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
                result.PreviewUrl = string.Format(_thumbnailImages
                    , _amazonS3Configuration.BucketName
                    , _amazonS3Configuration.Region
                    , file.ProductId
                    , file.Name);
                return result;
            }
            else
            {
                var result = ObjectMapper.Map<S3FileDto>(file);
                result.DownloadUrl = string.Format(_fileTypes
                    , _amazonS3Configuration.BucketName
                    , _amazonS3Configuration.Region
                    , file.ProductId
                    , file.Name);
                return result;
            }
        }
    }
}
