using layoutlovers.Authorization.Users;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace layoutlovers.DownloadAmazonS3Files
{
    public interface IDownloadAmazonS3FileManager : IAppManagerBase<DownloadAmazonS3File, Guid>
    {
        IQueryable<DownloadAmazonS3File> GetAllByCurrentDay(long userId);
        Task<DownloadAmazonS3File> SaveAsync(int editionId, Guid fileId, User user);
    }
}
