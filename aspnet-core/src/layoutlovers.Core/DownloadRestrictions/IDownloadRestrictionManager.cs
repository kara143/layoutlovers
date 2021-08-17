using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace layoutlovers.DownloadRestrictions
{
    public interface IDownloadRestrictionManager : IAppManagerBase<DownloadRestriction, Guid>
    {
        Task<List<DownloadRestriction>> GetAllListByEditionId(int editionId);
        IQueryable<DownloadRestriction> GetRestrictionsByEditionId(int editionId);
    }
}