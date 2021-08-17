using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.Domain.Repositories;

namespace layoutlovers.Editions
{
    public class EditionManager : AbpEditionManager
    {
        public const string DefaultEditionName = "Free";

        public EditionManager(
            IRepository<Edition> editionRepository,
            IAbpZeroFeatureValueStore featureValueStore)
            : base(
                editionRepository,
                featureValueStore
            )
        {
        }

        public async Task<List<Edition>> GetAllAsync()
        {
            return await EditionRepository.GetAllListAsync();
        }
    }
}
