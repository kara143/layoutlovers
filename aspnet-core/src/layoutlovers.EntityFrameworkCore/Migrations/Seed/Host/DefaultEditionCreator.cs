using System.Linq;
using Abp.Application.Features;
using Microsoft.EntityFrameworkCore;
using layoutlovers.Editions;
using layoutlovers.EntityFrameworkCore;
using layoutlovers.Features;
using System.Collections.Generic;

namespace layoutlovers.Migrations.Seed.Host
{
    public class DefaultEditionCreator
    {
        private readonly layoutloversDbContext _context;

        public DefaultEditionCreator(layoutloversDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateEditions();
        }

        private void CreateEditions()
        {
            var defaultEditions = _context.Editions.IgnoreQueryFilters().ToList();
            //.FirstOrDefault(e => e.Name == EditionManager.DefaultEditionName);
            if (defaultEditions.Count == 0)
            {
                defaultEditions.Add(new SubscribableEdition 
                {
                    Name = EditionManager.DefaultEditionName,
                    DisplayName = EditionManager.DefaultEditionName 
                });

                defaultEditions.Add( new SubscribableEdition
                {
                    Name = "Basic",
                    DisplayName = "Basic",
                    MonthlyPrice = 50,
                    TrialDayCount = 0,
                    WaitingDayAfterExpire = 0,
                    AnnualPrice = 0,
                    DailyPrice = 0,
                    WeeklyPrice = 0
                });

                defaultEditions.Add(new SubscribableEdition
                {
                    Name = "Premium",
                    DisplayName = "Premium",
                    MonthlyPrice = 100,
                    TrialDayCount = 0,
                    WaitingDayAfterExpire = 0,
                    AnnualPrice = 0,
                    DailyPrice = 0,
                    WeeklyPrice = 0
                });

                defaultEditions.Add(new SubscribableEdition
                {
                    Name = "Premium Plus",
                    DisplayName = "Premium Plus",
                    MonthlyPrice = 1000,
                    TrialDayCount = 0,
                    WaitingDayAfterExpire = 0,
                    AnnualPrice = 0,
                    DailyPrice = 0,
                    WeeklyPrice = 0
                });

                _context.Editions.AddRange(defaultEditions);
                _context.SaveChanges();

                /* Add desired features to the standard edition, if wanted... */
            }

            foreach (var defaultEdition in defaultEditions)
            {
                if (defaultEdition.Id > 0)
                {
                    CreateFeatureIfNotExists(defaultEdition.Id, AppFeatures.ChatFeature, true);
                    CreateFeatureIfNotExists(defaultEdition.Id, AppFeatures.TenantToTenantChatFeature, true);
                    CreateFeatureIfNotExists(defaultEdition.Id, AppFeatures.TenantToHostChatFeature, true);
                }
            }
           
        }

        private void CreateFeatureIfNotExists(int editionId, string featureName, bool isEnabled)
        {
            var defaultEditionChatFeature = _context.EditionFeatureSettings.IgnoreQueryFilters()
                                                        .FirstOrDefault(ef => ef.EditionId == editionId && ef.Name == featureName);

            if (defaultEditionChatFeature == null)
            {
                _context.EditionFeatureSettings.Add(new EditionFeatureSetting
                {
                    Name = featureName,
                    Value = isEnabled.ToString().ToLower(),
                    EditionId = editionId
                });
            }
        }
    }
}