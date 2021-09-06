using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using Abp.Timing;
using Abp.UI;
using layoutlovers.Amazon;
using layoutlovers.Authorization.Users;
using layoutlovers.Authorization.Users.EmailModels;
using layoutlovers.Jobs.Configuration;
using layoutlovers.LayoutProducts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace layoutlovers.Jobs
{
    public class MakeNewProductPassiveWorker : AsyncPeriodicBackgroundWorkerBase, ISingletonDependency
    {
        private readonly ILayoutProductManager _layoutProductManager;
        private readonly IRepository<User, long> _userRepository;
        private readonly AmazonS3Configuration _amazonS3Configuration;
        private readonly UserEmailer _userEmailer;
        private readonly JobsConfiguration _jobsConfiguration;

        private const int PeriodWeek = 1 * 60 * 60 * 1000 * 24 * 7;
        public MakeNewProductPassiveWorker(AbpAsyncTimer timer
            , ILayoutProductManager layoutProductManager
            , IRepository<User, long> userRepository
            , UserEmailer userEmailer
            , AmazonS3Configuration amazonS3Configuration
            , JobsConfiguration jobsConfiguration
            ) : base(timer)
        {
            Timer.Period = PeriodWeek;
            _layoutProductManager = layoutProductManager;
            _userRepository = userRepository;
            _userEmailer = userEmailer;
            _amazonS3Configuration = amazonS3Configuration;
            _jobsConfiguration = jobsConfiguration;
        }

        [UnitOfWork]
        protected override async Task DoWorkAsync()
        {
            if (!_jobsConfiguration.IsNewProductsNotification)
            {
                return;
            }

            var failedTenancyNames = new List<string>();
            try
            {
                using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
                {
                    var oneWeekAgo = Clock.Now.Subtract(TimeSpan.FromDays(2));

                    var newProductList = _layoutProductManager.GetAllIncluding(f => f.AmazonS3Files)
                        .Where(f => f.CreationTime > oneWeekAgo)
                        .ToList()
                        .Select((value, index) => new LayoutProductWithPreviewUrls
                        {
                            LayoutProduct = value,
                            PreviewUrls = value.AmazonS3Files
                            .Select(a => string.Format(_amazonS3Configuration.ThumbnailImages
                            , _amazonS3Configuration.BucketName
                            , _amazonS3Configuration.Region
                            , a.LayoutProductId
                            , a.Name))
                        })
                        .ToList();

                    var activeUsers = await _userRepository.GetAll()
                        .Where(u => u.IsActive)
                        .ToListAsync();

                    await TrySendNotification(activeUsers, newProductList);

                    CurrentUnitOfWork.SaveChanges();
                }
            }
            catch (Exception exception)
            {
                throw new UserFriendlyException(exception.Message);
            }
            
        }

        private async Task TrySendNotification(List<User> users, List<LayoutProductWithPreviewUrls> layoutProductWithPreviewUrls)
        {
            if (users.IsNullOrEmpty() && layoutProductWithPreviewUrls.IsNullOrEmpty())
            {
                return;
            }

            foreach (var item in users)
            {
                await _userEmailer.SendNotificationAboutNewProducts(item, layoutProductWithPreviewUrls);
            }
        }
    }
}
