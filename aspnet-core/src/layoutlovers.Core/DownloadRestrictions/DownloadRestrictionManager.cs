using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace layoutlovers.DownloadRestrictions
{
    public class DownloadRestrictionManager : AppManagerBase<DownloadRestriction, Guid>, IDownloadRestrictionManager
    {
        public DownloadRestrictionManager(IRepository<DownloadRestriction, Guid> repository) : base(repository)
        {
        }

        public async Task<List<DownloadRestriction>> GetAllListByEditionId(int editionId)
        {
            return await _repository.GetAllListAsync(f => f.SubscribableEditionId == editionId);
        }
        public IQueryable<DownloadRestriction> GetRestrictionsByEditionId(int editionId)
        {
           return _repository.GetAll().Where(f => f.SubscribableEditionId == editionId);
        }
    }
}
