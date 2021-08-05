using System.Linq;
using Abp.Configuration;
using Abp.Localization;
using Abp.MultiTenancy;
using Abp.Net.Mail;
using Microsoft.EntityFrameworkCore;
using layoutlovers.EntityFrameworkCore;

namespace layoutlovers.Migrations.Seed.Host
{
    public class DefaultSettingsCreator
    {
        private readonly layoutloversDbContext _context;

        public DefaultSettingsCreator(layoutloversDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            int? tenantId = null;

            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (!layoutloversConsts.MultiTenancyEnabled)
#pragma warning disable 162
            {
                tenantId = MultiTenancyConsts.DefaultTenantId;
            }
#pragma warning restore 162

            //Emailing
            //AddSettingIfNotExists(EmailSettingNames.DefaultFromAddress, "layoutlovers@outlook.com", tenantId);
            //AddSettingIfNotExists(EmailSettingNames.DefaultFromDisplayName, "layoutlovers@outlook.com", tenantId);

            AddSettingIfNotExists(EmailSettingNames.DefaultFromAddress, "admin@mydomain.com", tenantId);
            AddSettingIfNotExists(EmailSettingNames.DefaultFromDisplayName, "mydomain.com mailer", tenantId);

            //Languages
            AddSettingIfNotExists(LocalizationSettingNames.DefaultLanguage, "en", tenantId);

            //New registered tenants are active by default.
            //AddSettingIfNotExists("App.TenantManagement.IsNewRegisteredTenantActiveByDefault", "true");
            //AddSettingIfNotExists("App.UserManagement.SessionTimeOut.ShowLockScreenWhenTimedOut", "false");
            //AddSettingIfNotExists("Abp.Zero.UserManagement.PasswordComplexity.RequireDigit", "false");
            //AddSettingIfNotExists("Abp.Zero.UserManagement.PasswordComplexity.RequireLowercase", "false");
            //AddSettingIfNotExists("Abp.Zero.UserManagement.PasswordComplexity.RequireNonAlphanumeric", "false");
            //AddSettingIfNotExists("Abp.Zero.UserManagement.PasswordComplexity.RequireUppercase", "false");
            //AddSettingIfNotExists("App.UserManagement.AllowOneConcurrentLoginPerUser", "true");
            //AddSettingIfNotExists("Abp.Net.Mail.Smtp.Password", "Y7585fLpUC6pOb5T2IVVGo4oDHU3wipNxdLfBs7XUUg=");

            //AddSettingIfNotExists("App.TenantManagement.IsNewRegisteredTenantActiveByDefault", "true");
            //AddSettingIfNotExists("Abp.Zero.UserManagement.IsEmailConfirmationRequiredForLogin", "true");
            //AddSettingIfNotExists("App.UserManagement.IsCookieConsentEnabled", "true");
            //AddSettingIfNotExists("Abp.Zero.UserManagement.TwoFactorLogin.IsEnabled", "true");
            //AddSettingIfNotExists("Abp.Zero.UserManagement.TwoFactorLogin.IsSmsProviderEnabled", "false");
            //AddSettingIfNotExists("Abp.Net.Mail.Smtp.Host", "smtp-mail.outlook.com");
            //AddSettingIfNotExists("Abp.Net.Mail.Smtp.Port", "587");
            //AddSettingIfNotExists("Abp.Net.Mail.Smtp.UserName", "layoutlovers@outlook.com");
            //AddSettingIfNotExists("Abp.Net.Mail.Smtp.Domain", "smtp-mail.outlook.com");
            //AddSettingIfNotExists("Abp.Net.Mail.Smtp.EnableSsl", "true");
            //AddSettingIfNotExists("Abp.Net.Mail.Smtp.UseDefaultCredentials", "false");

        }

        private void AddSettingIfNotExists(string name, string value, int? tenantId = null)
        {
            if (_context.Settings.IgnoreQueryFilters().Any(s => s.Name == name && s.TenantId == tenantId && s.UserId == null))
            {
                return;
            }

            _context.Settings.Add(new Setting(tenantId, null, name, value));
            _context.SaveChanges();
        }
    }
}