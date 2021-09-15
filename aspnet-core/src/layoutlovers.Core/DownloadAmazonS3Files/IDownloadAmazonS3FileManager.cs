using layoutlovers.Authorization.Users;
using layoutlovers.LayoutProducts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace layoutlovers.DownloadAmazonS3Files
{
    public interface IDownloadAmazonS3FileManager : IAppManagerBase<DownloadAmazonS3File, Guid>
    {
        IQueryable<LayoutProduct> DownloadDuringSubscriptionByUserId(long id);
        IQueryable<DownloadAmazonS3File> GetAllByCurrentDay(long userId);
        Task<DownloadAmazonS3File> SaveAsync(int editionId, Guid fileId, User user);
    }
}
