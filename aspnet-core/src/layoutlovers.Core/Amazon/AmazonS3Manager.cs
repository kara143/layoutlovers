using Abp.Domain.Repositories;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using layoutlovers.Files;
using layoutlovers.Products;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace layoutlovers.Amazon
{
    public class AmazonS3Manager : AppManagerBase<AmazonS3File, Guid>, IAmazonS3Manager
    {
        private readonly AmazonS3Client _client;
        private readonly AmazonS3Configuration _amazonS3Configuration;
        private readonly IRepository<Product, Guid> _product;
        public AmazonS3Manager(IRepository<AmazonS3File, Guid> repository
            , AmazonS3Configuration amazonS3Configuration
            , IRepository<Product, Guid> product) : base(repository)
        {
            _amazonS3Configuration = amazonS3Configuration;
            _client = new AmazonS3Client(_amazonS3Configuration.AwsAccessKeyId
                , _amazonS3Configuration.AwsSecretAccessKey
                , RegionEndpoint.USEast1);
            _product = product;
        }
        //TODO: Transfer to another manager!
        #region AmazonS3
        public async Task DeleteFromS3(AmazonS3File file)
        {
            try
            {
                string buckentName = GetBucketName(file.IsImage, file.ProductId.ToString());

                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = buckentName,
                    Key = file.Name,
                };

                await _client.DeleteObjectAsync(deleteObjectRequest);
            }
            catch (Exception)
            {
                throw new Exception($"An error occurred while deleting a file from the product with ID: {file.ProductId} and file key: {file.Name} ABS");
            }
        }
        public async Task UploadToS3(IFormFile file, string productId)
        {
            string bucketName = GetBucketName(file.HasImage(), productId);

            try
            {
                using (var newMemoryStream = new MemoryStream())
                {
                    file.CopyTo(newMemoryStream);

                    var fileTransferUtility = new TransferUtility(_client);
                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        InputStream = newMemoryStream,
                        Key = file.FileName,
                        BucketName = bucketName,
                        CannedACL = S3CannedACL.PublicReadWrite
                    };
                    await fileTransferUtility.UploadAsync(uploadRequest);
                }
            }
            catch (Exception)
            {
                throw new Exception($"An error occurred while uploading a file to the AWS In product ID: {productId} and key: {file.FileName}.");
            }

        }
        #endregion
        public async Task DeleteFromS3AndDbById(Guid id)
        {
            var file = await _repository.GetAsync(id);
            await DeleteFromS3(file);
            await DeleteAsync(file.Id);
        }

        public async Task DleteAllByProductId(Guid id)
        {
            var fileIds = _repository.GetAll()
                .Where(f => f.ProductId == id)
                .Select(f => f.Id)
                .ToList();

            foreach (var fileId in fileIds)
            {
                await DeleteFromS3AndDbById(fileId);
            }
        }

        public async Task<AmazonS3File> UploadToS3AndInsert(IFormFile file, Guid productId)
        {
            var product = _product.GetAllIncluding(f => f.AmazonS3Files)
                .FirstOrDefault(f => f.Id == productId);

            if (product == null)
            {
                throw new Exception($"The product with Id: {productId} wos not found.");
            }

            //Сheck if the given product has a file with the given name.
            var s3File = product.AmazonS3Files.FirstOrDefault(f => f.Name.Equals(file.FileName));
            if (s3File != null)
            {
                //Delete the file from the database if the file already exists!
                await _repository.DeleteAsync(s3File.Id);
            }

            await UploadToS3(file, productId.ToString());
            return await MapToEntityAndInsert(file, productId);
        }

        public IEnumerable<IAmazonS3File> GetAllByProductId(Guid id)
        {
            return _repository.GetAll().Where(f => f.ProductId == id).ToArray();
        }

        public async Task<IAmazonS3File> GetById(Guid id)
        {
            return await _repository.GetAsync(id);
        }

        private async Task<AmazonS3File> MapToEntityAndInsert(IFormFile file, Guid productId)
        {
            var amazonS3File = new AmazonS3File
            {
                Name = file.FileName,
                FileExtension = file.GetFileType(),
                ProductId = productId,
                IsImage = file.HasImage()
            };
            return await InsertAndGetEntityAsync(amazonS3File);
        }

        private string GetBucketName(bool isImage, string productId)
        {
            if (isImage)
            {
                return $"{_amazonS3Configuration.BucketName}/{productId}/Thumbnail images";
            }

            return $"{_amazonS3Configuration.BucketName}/{productId}/File types"; ;
        }
    }
}
