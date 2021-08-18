using Abp.Domain.Repositories;
using layoutlovers.Amazon;
using layoutlovers.Authorization.Users;
using layoutlovers.DownloadRestrictions;
using layoutlovers.Extensions;
using layoutlovers.LayoutProducts;
using layoutlovers.Purchases;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace layoutlovers.DownloadAmazonS3Files
{
    public class DownloadAmazonS3FileManager : AppManagerBase<DownloadAmazonS3File, Guid>, IDownloadAmazonS3FileManager
    {
        private readonly UserManager _userManager;
        private readonly IDownloadRestrictionManager _downloadRestrictionManager;
        private readonly IPurchaseManager _purchaseManager;
        private readonly IAmazonS3Manager _amazonS3Manager;
        public DownloadAmazonS3FileManager(IRepository<DownloadAmazonS3File, Guid> repository
            , UserManager userManager
            , IDownloadRestrictionManager downloadRestrictionManager
            , IPurchaseManager purchaseManager
            , IAmazonS3Manager amazonS3Manager
            ) : base(repository)
        {
            _userManager = userManager;
            _downloadRestrictionManager = downloadRestrictionManager;
            _purchaseManager = purchaseManager;
            _amazonS3Manager = amazonS3Manager;
        }

        /// <summary>
        /// We will save the download details and return the download url!
        /// </summary>
        /// <param name="editionId">
        /// Payment plan ID
        /// </param>
        /// <param name="fileId">
        /// File ID to download
        /// </param>
        /// <param name="user">
        /// User who will upload the file
        /// </param>
        /// <returns></returns>
        public async Task<DownloadAmazonS3File> SaveAsync(int editionId, Guid fileId, User user)//TODO: Consider renameng
        {
            if (user.IsNull())
            {
                throw new Exception("User cannot be null.");
            }

            var userId = user.Id;

            //check for the existence of the file.
            //Including we get the product in order to avoid an additional request to the database to receive it
            var file = await _amazonS3Manager.GetAllIncluding(f => f.LayoutProduct)
                .Where(f => !f.IsImage)
                .FirstOrDefaultAsync(f => f.Id == fileId);

            if (file.IsNull())
            {
                throw new Exception($"File with Id {fileId} not found, or has permission not available for download.");
            }

            //Check whether the product to which this file belongs was purchased.
            var purchase = await _purchaseManager.GetAllByUserId(userId)
                .FirstOrDefaultAsync(f => f.LayoutProductId == file.LayoutProductId);

            if (purchase.IsNull())
            {
                throw new Exception($"The file with id {fileId} cannot be downloaded because the product " +
                    $"to which it was attached was not previously purchased!");
            }

            var product = file.LayoutProduct;
            //Check if this type is available for download to the user.
            var restriction = await _downloadRestrictionManager.GetRestrictionsByEditionId(editionId)
                .FirstOrDefaultAsync(f => f.LayoutProductType == product.LayoutProductType);

            if (restriction.IsNull())
            {
                throw new Exception($"The user with ID {userId} does not have permission to download this product!" +
                    $" To download this product, you need to go to the plan above!");
            }

            //Products downloaded by the current user today
            var products = GetProductsByCurrentDay(userId);
            var productsCount = products.Count();

            //We check if the daily download limit has been exceeded
            if (productsCount >= restriction.DownloadPerDay)
            {
                throw new Exception($"The user with Id {userId} has exceeded the daily download limit.");
            }

            //Save information about the current download.
            //Wd do not save the url for download as it is generated dynamically
            var downloadProduct = await InsertAsync(new DownloadAmazonS3File
            {
                UserId = userId,
                AmazonS3FileId = fileId
            });

            //Update the number of downloaded files for the user today!
            if (!product.LayoutProductType.IsFree())
            {
                user.DownloadToday = productsCount;
                await _userManager.UpdateAsync(user);
            }

            return downloadProduct;

        }

        public IQueryable<DownloadAmazonS3File> GetAllByCurrentDay(long userId)
        {
            var currentDate = DateTime.Now.Date;
            return _repository.GetAll()
                .Where(f => f.CreationTime.Date == currentDate)
                .Where(f => f.UserId == userId);
        }

        private IEnumerable<LayoutProduct> GetProductsByCurrentDay(long userId)
        {
            var downloadAmazonS3File = GetAllByCurrentDay(userId);
            var products = downloadAmazonS3File
                .Include(f => f.AmazonS3File)
                .Select(f => f.AmazonS3File)
                .Select(f => f.LayoutProduct)
                .ToList()//TODO: In the future, remove the conversion to IEnumerable.
                .GroupBy(f => f.Id)
                .Select(f => f.First())
                .ToList();

            return products;
        }
    }
}
