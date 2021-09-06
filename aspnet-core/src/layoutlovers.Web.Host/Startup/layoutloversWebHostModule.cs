﻿using System.Collections.Generic;
using Abp.AspNetZeroCore;
using Abp.AspNetZeroCore.Web.Authentication.External;
using Abp.AspNetZeroCore.Web.Authentication.External.Facebook;
using Abp.AspNetZeroCore.Web.Authentication.External.Google;
using Abp.AspNetZeroCore.Web.Authentication.External.Microsoft;
using Abp.AspNetZeroCore.Web.Authentication.External.OpenIdConnect;
using Abp.AspNetZeroCore.Web.Authentication.External.Twitter;
using Abp.AspNetZeroCore.Web.Authentication.External.WsFederation;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Extensions;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Threading.BackgroundWorkers;
using Abp.Timing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using layoutlovers.Auditing;
using layoutlovers.Configuration;
using layoutlovers.EntityFrameworkCore;
using layoutlovers.MultiTenancy;
using layoutlovers.Web.Startup.ExternalLoginInfoProviders;
using System;
using layoutlovers.Jobs;

namespace layoutlovers.Web.Startup
{
    [DependsOn(
        typeof(layoutloversWebCoreModule)
    )]
    public class layoutloversWebHostModule : AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public layoutloversWebHostModule(
            IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            Configuration.Modules.AbpWebCommon().MultiTenancy.DomainFormat =
                _appConfiguration["App:ServerRootAddress"] ?? "https://localhost:44301/";
            Configuration.Modules.AspNetZero().LicenseCode = _appConfiguration["AbpZeroLicenseCode"];
            //TODO: transfer to some settings
            Configuration.BackgroundJobs.IsJobExecutionEnabled = true;
            Configuration.BackgroundJobs.UserTokenExpirationPeriod = TimeSpan.FromHours(1);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(layoutloversWebHostModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            using (var scope = IocManager.CreateScope())
            {
                if (!scope.Resolve<DatabaseCheckHelper>().Exist(_appConfiguration["ConnectionStrings:Default"]))
                {
                    return;
                }
            }

            var workManager = IocManager.Resolve<IBackgroundWorkerManager>();
            if (IocManager.Resolve<IMultiTenancyConfig>().IsEnabled)
            {
                workManager.Add(IocManager.Resolve<SubscriptionExpirationCheckWorker>());
                workManager.Add(IocManager.Resolve<SubscriptionExpireEmailNotifierWorker>());
                workManager.Add(IocManager.Resolve<MakeNewProductPassiveWorker>());
            }

            if (Configuration.Auditing.IsEnabled && ExpiredAuditLogDeleterWorker.IsEnabled)
            {
                workManager.Add(IocManager.Resolve<ExpiredAuditLogDeleterWorker>());
            }

            ConfigureExternalAuthProviders();
        }

        private void ConfigureExternalAuthProviders()
        {
            var externalAuthConfiguration = IocManager.Resolve<ExternalAuthConfiguration>();

            if (bool.Parse(_appConfiguration["Authentication:OpenId:IsEnabled"]))
            {
                if (bool.Parse(_appConfiguration["Authentication:AllowSocialLoginSettingsPerTenant"]))
                {
                    externalAuthConfiguration.ExternalLoginInfoProviders.Add(
                        IocManager.Resolve<TenantBasedOpenIdConnectExternalLoginInfoProvider>());
                }
                else
                {
                    var jsonClaimMappings = new List<JsonClaimMap>();
                    _appConfiguration.GetSection("Authentication:OpenId:ClaimsMapping").Bind(jsonClaimMappings);

                    externalAuthConfiguration.ExternalLoginInfoProviders.Add(
                        new OpenIdConnectExternalLoginInfoProvider(
                            _appConfiguration["Authentication:OpenId:ClientId"],
                            _appConfiguration["Authentication:OpenId:ClientSecret"],
                            _appConfiguration["Authentication:OpenId:Authority"],
                            _appConfiguration["Authentication:OpenId:LoginUrl"],
                            bool.Parse(_appConfiguration["Authentication:OpenId:ValidateIssuer"]),
                            jsonClaimMappings
                        )
                    );
                }
            }

            if (bool.Parse(_appConfiguration["Authentication:WsFederation:IsEnabled"]))
            {
                if (bool.Parse(_appConfiguration["Authentication:AllowSocialLoginSettingsPerTenant"]))
                {
                    externalAuthConfiguration.ExternalLoginInfoProviders.Add(
                        IocManager.Resolve<TenantBasedWsFederationExternalLoginInfoProvider>());
                }
                else
                {
                    var jsonClaimMappings = new List<JsonClaimMap>();
                    _appConfiguration.GetSection("Authentication:WsFederation:ClaimsMapping").Bind(jsonClaimMappings);

                    externalAuthConfiguration.ExternalLoginInfoProviders.Add(
                        new WsFederationExternalLoginInfoProvider(
                            _appConfiguration["Authentication:WsFederation:ClientId"],
                            _appConfiguration["Authentication:WsFederation:Tenant"],
                            _appConfiguration["Authentication:WsFederation:MetaDataAddress"],
                            _appConfiguration["Authentication:WsFederation:Authority"],
                            jsonClaimMappings
                        )
                    );
                }
            }

            if (bool.Parse(_appConfiguration["Authentication:Facebook:IsEnabled"]))
            {
                if (bool.Parse(_appConfiguration["Authentication:AllowSocialLoginSettingsPerTenant"]))
                {
                    externalAuthConfiguration.ExternalLoginInfoProviders.Add(
                        IocManager.Resolve<TenantBasedFacebookExternalLoginInfoProvider>());
                }
                else
                {
                    externalAuthConfiguration.ExternalLoginInfoProviders.Add(new FacebookExternalLoginInfoProvider(
                        _appConfiguration["Authentication:Facebook:AppId"],
                        _appConfiguration["Authentication:Facebook:AppSecret"]
                    ));
                }
            }

            if (bool.Parse(_appConfiguration["Authentication:Twitter:IsEnabled"]))
            {
                if (bool.Parse(_appConfiguration["Authentication:AllowSocialLoginSettingsPerTenant"]))
                {
                    externalAuthConfiguration.ExternalLoginInfoProviders.Add(
                        IocManager.Resolve<TenantBasedTwitterExternalLoginInfoProvider>());
                }
                else
                {
                    var twitterExternalLoginInfoProvider = new TwitterExternalLoginInfoProvider(
                        _appConfiguration["Authentication:Twitter:ConsumerKey"],
                        _appConfiguration["Authentication:Twitter:ConsumerSecret"],
                        _appConfiguration["App:ClientRootAddress"].EnsureEndsWith('/') + "account/login"
                    );

                    externalAuthConfiguration.ExternalLoginInfoProviders.Add(twitterExternalLoginInfoProvider);
                }
            }

            if (bool.Parse(_appConfiguration["Authentication:Google:IsEnabled"]))
            {
                if (bool.Parse(_appConfiguration["Authentication:AllowSocialLoginSettingsPerTenant"]))
                {
                    externalAuthConfiguration.ExternalLoginInfoProviders.Add(
                        IocManager.Resolve<TenantBasedGoogleExternalLoginInfoProvider>());
                }
                else
                {
                    externalAuthConfiguration.ExternalLoginInfoProviders.Add(
                        new GoogleExternalLoginInfoProvider(
                            _appConfiguration["Authentication:Google:ClientId"],
                            _appConfiguration["Authentication:Google:ClientSecret"],
                            _appConfiguration["Authentication:Google:UserInfoEndpoint"]
                        )
                    );
                }
            }

            if (bool.Parse(_appConfiguration["Authentication:Microsoft:IsEnabled"]))
            {
                if (bool.Parse(_appConfiguration["Authentication:AllowSocialLoginSettingsPerTenant"]))
                {
                    externalAuthConfiguration.ExternalLoginInfoProviders.Add(
                        IocManager.Resolve<TenantBasedMicrosoftExternalLoginInfoProvider>());
                }
                else
                {
                    externalAuthConfiguration.ExternalLoginInfoProviders.Add(
                        new MicrosoftExternalLoginInfoProvider(
                            _appConfiguration["Authentication:Microsoft:ConsumerKey"],
                            _appConfiguration["Authentication:Microsoft:ConsumerSecret"]
                        )
                    );
                }
            }
        }
    }
}
