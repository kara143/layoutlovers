using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.Domain.Repositories;
using layoutlovers.Extensions;

namespace layoutlovers.Editions
{
    public class EditionManager : AbpEditionManager
    {
        public const string DefaultEditionName = "Free";
        private readonly IRepository<SubscribableEdition> _subscribableEditionRepository;
        public EditionManager(
            IRepository<Edition> editionRepository
            ,IAbpZeroFeatureValueStore featureValueStore
            , IRepository<SubscribableEdition> subscribableEditionRepository
            )
            : base(
                editionRepository,
                featureValueStore
            )
        {
            _subscribableEditionRepository = subscribableEditionRepository;
        }

        public async Task<List<Edition>> GetAllAsync()
        {
            return await EditionRepository.GetAllListAsync();
        }

        public async Task<bool> IsFree(int editionId)
        {
            var edition = await _subscribableEditionRepository.GetAsync(editionId);
            if (edition.IsNull())
            {
                throw new Exception($"Edition with Id {editionId} not found.");
            }
            return edition.IsFree;
        }
    }
}
