using Abp.Domain.Repositories;
using Abp.UI;
using layoutlovers.Amazon;
using layoutlovers.Authorization.Users;
using layoutlovers.DownloadRestrictions;
using layoutlovers.Editions;
using layoutlovers.Extensions;
using layoutlovers.LayoutProducts;
using layoutlovers.PurchaseItems;
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
        private readonly IPurchaseItemManager _purchaseManager;
        private readonly IAmazonS3Manager _amazonS3Manager;
        private readonly EditionManager _editionManager;
        public DownloadAmazonS3FileManager(IRepository<DownloadAmazonS3File, Guid> repository
            , UserManager userManager
            , IDownloadRestrictionManager downloadRestrictionManager
            , IPurchaseItemManager purchaseManager
            , IAmazonS3Manager amazonS3Manager
            , EditionManager editionManager
            ) : base(repository)
        {
            _userManager = userManager;
            _downloadRestrictionManager = downloadRestrictionManager;
            _purchaseManager = purchaseManager;
            _amazonS3Manager = amazonS3Manager;
            _editionManager = editionManager;
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
                throw new UserFriendlyException("User cannot be null.");
            }

            var userId = user.Id;

            //check for the existence of the file.
            //Including we get the product in order to avoid an additional request to the database to receive it
            var file = await _amazonS3Manager.GetAllIncluding(f => f.LayoutProduct)
                .Where(f => !f.IsImage)
                .FirstOrDefaultAsync(f => f.Id == fileId);

            if (file.IsNull())
            {
                throw new UserFriendlyException($"File with Id {fileId} not found, or has permission not available for download.");
            }

            var isFree = await _editionManager.IsFree(editionId);
            DownloadAmazonS3File downloadAmazonS3File = null;
            if (isFree)
            {
                downloadAmazonS3File = await SaveToFreeEdition(userId, file);
                return downloadAmazonS3File;
            }
            downloadAmazonS3File = await SaveToPaidVersion(editionId, file, user);
            
            file.CountDownloads = await _amazonS3Manager.GetAll()
                .Where(f => f.Id == file.Id)
                .CountAsync();

            await _amazonS3Manager.InsertOrUpdateAsync(file);
            return downloadAmazonS3File;
        }

        private async Task<DownloadAmazonS3File> SaveToFreeEdition(long userId, AmazonS3File file)
        {
            //Check whether the product to which this file belongs was purchased.
            var purchase = await _purchaseManager.GetAllByUserId(userId)
                .FirstOrDefaultAsync(f => f.LayoutProductId == file.LayoutProductId);

            if (purchase.IsNull())
            {
                throw new UserFriendlyException($"The file with id {file.Id} cannot be downloaded because the product " +
                    $"to which it was attached was not previously purchased!");
            }

            //Save information about the current download.
            //Wd do not save the url for download as it is generated dynamically
            var downloadProduct = await InsertAsync(new DownloadAmazonS3File
            {
                UserId = userId,
                AmazonS3FileId = file.Id,
            });

            //We do not check the number of downloads since the free subscription has no limit
            return downloadProduct;
        }

        private async Task<DownloadAmazonS3File> SaveToPaidVersion(int editionId, AmazonS3File file, User user)
        {
            var userId = user.Id;
            var product = file.LayoutProduct;
            //Check if this type is available for download to the user.
            var restriction = await _downloadRestrictionManager.GetRestrictionsByEditionId(editionId)
                .FirstOrDefaultAsync(f => f.LayoutProductType == product.LayoutProductType);

            if (restriction.IsNull())
            {
                throw new UserFriendlyException($"The user with ID {userId} does not have permission to download this product!" +
                    $" To download this product, you need to go to the plan above!");
            }

            //Products downloaded by the current user today
            var products = GetProductsByCurrentDay(userId);
            var productsCount = products.Count();

            //We check if the daily download limit has been exceeded
            if (productsCount >= restriction.DownloadPerDay)
            {
                throw new UserFriendlyException($"The user with Id {userId} has exceeded the daily download limit.");
            }

            //Save information about the current download.
            //Wd do not save the url for download as it is generated dynamically
            var downloadProduct = await InsertAsync(new DownloadAmazonS3File
            {
                UserId = userId,
                AmazonS3FileId = file.Id,
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
