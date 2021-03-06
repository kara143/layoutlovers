using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Configuration;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Localization;
using Abp.Net.Mail;
using layoutlovers.Chat;
using layoutlovers.Editions;
using layoutlovers.Localization;
using layoutlovers.MultiTenancy;
using System.Net.Mail;
using System.Web;
using Abp.Runtime.Security;
using Abp.Runtime.Session;
using Abp.UI;
using layoutlovers.Net.Emailing;
using layoutlovers.Purchases;
using layoutlovers.PurchaseItems;
using System.Linq;
using layoutlovers.Authorization.Users.EmailModels;
using layoutlovers.MultiTenancy.Payments;

namespace layoutlovers.Authorization.Users
{
    /// <summary>
    /// Used to send email to users.
    /// </summary>
    public class UserEmailer : layoutloversServiceBase, IUserEmailer, ITransientDependency
    {
        private readonly IEmailTemplateProvider _emailTemplateProvider;
        private readonly IEmailSender _emailSender;
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly ICurrentUnitOfWorkProvider _unitOfWorkProvider;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly ISettingManager _settingManager;
        private readonly EditionManager _editionManager;
        private readonly UserManager _userManager;
        private readonly IAbpSession _abpSession;

        // used for styling action links on email messages.
        private string _emailButtonStyle =
            "padding-left: 30px; padding-right: 30px; padding-top: 12px; padding-bottom: 12px; color: #ffffff; background-color: #00bb77; font-size: 14pt; text-decoration: none;";
        private string _emailButtonColor = "#00bb77";

        public UserEmailer(
            IEmailTemplateProvider emailTemplateProvider,
            IEmailSender emailSender,
            IRepository<Tenant> tenantRepository,
            ICurrentUnitOfWorkProvider unitOfWorkProvider,
            IUnitOfWorkManager unitOfWorkManager,
            ISettingManager settingManager,
            EditionManager editionManager,
            UserManager userManager,
            IAbpSession abpSession)
        {
            _emailTemplateProvider = emailTemplateProvider;
            _emailSender = emailSender;
            _tenantRepository = tenantRepository;
            _unitOfWorkProvider = unitOfWorkProvider;
            _unitOfWorkManager = unitOfWorkManager;
            _settingManager = settingManager;
            _editionManager = editionManager;
            _userManager = userManager;
            _abpSession = abpSession;
        }

        /// <summary>
        /// Do not use
        /// Send email activation link to user's email address.
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="link">Email activation link</param>
        /// <param name="plainPassword">
        /// Can be set to user's plain password to include it in the email.
        /// </param>
        [UnitOfWork]
        public virtual async Task SendEmailActivationLinkAsync(User user, string link, string plainPassword = null)
        {
            await CheckMailSettingsEmptyOrNull();

            if (user.EmailConfirmationCode.IsNullOrEmpty())
            {
                throw new Exception("EmailConfirmationCode should be set in order to send email activation link.");
            }

            link = link.Replace("{userId}", user.Id.ToString());
            link = link.Replace("{confirmationCode}", Uri.EscapeDataString(user.EmailConfirmationCode));

            if (user.TenantId.HasValue)
            {
                link = link.Replace("{tenantId}", user.TenantId.ToString());
            }

            link = EncryptQueryParameters(link);

            var tenancyName = GetTenancyNameOrNull(user.TenantId);
            var emailTemplate = GetTitleAndSubTitle(user.TenantId, L("EmailActivation_Title"), L("EmailActivation_SubTitle"));
            var mailMessage = new StringBuilder();

            mailMessage.AppendLine("<b>" + L("NameSurname") + "</b>: " + user.Name + " " + user.Surname + "<br />");

            if (!tenancyName.IsNullOrEmpty())
            {
                mailMessage.AppendLine("<b>" + L("TenancyName") + "</b>: " + tenancyName + "<br />");
            }

            mailMessage.AppendLine("<b>" + L("UserName") + "</b>: " + user.UserName + "<br />");

            if (!plainPassword.IsNullOrEmpty())
            {
                mailMessage.AppendLine("<b>" + L("Password") + "</b>: " + plainPassword + "<br />");
            }

            mailMessage.AppendLine("<br />");
            mailMessage.AppendLine(L("EmailActivation_ClickTheLinkBelowToVerifyYourEmail") + "<br /><br />");
            mailMessage.AppendLine("<a style=\"" + _emailButtonStyle + "\" bg-color=\"" + _emailButtonColor + "\" href=\"" + link + "\">" + L("Verify") + "</a>");
            mailMessage.AppendLine("<br />");
            mailMessage.AppendLine("<br />");
            mailMessage.AppendLine("<br />");
            mailMessage.AppendLine("<span style=\"font-size: 9pt;\">" + L("EmailMessage_CopyTheLinkBelowToYourBrowser") + "</span><br />");
            mailMessage.AppendLine("<span style=\"font-size: 8pt;\">" + link + "</span>");

            await ReplaceBodyAndSend(user.EmailAddress, L("EmailActivation_Subject"), emailTemplate, mailMessage);
        }
        /// <summary>
        /// Send email activation link to user's email address.
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="link">Email activation link</param>
        /// <param name="plainPassword">
        /// Can be set to user's plain password to include it in the email.
        /// </param>
        [UnitOfWork]
        public virtual async Task SendEmailActivationLink(User user, string link, string plainPassword = null)
        {
            await CheckMailSettingsAnduserEmail(user);

            link = link.Replace("{userId}", user.Id.ToString());
            link = link.Replace("{confirmationCode}", Uri.EscapeDataString(user.EmailConfirmationCode));

            if (user.TenantId.HasValue)
            {
                link = link.Replace("{tenantId}", user.TenantId.ToString());
            }

            link = EncryptQueryParameters(link);

            var emailTemplate = GetTemplate("layoutlovers.Net.Emailing.EmailTemplates.ResetPassword.resetPassword.html"
                , L("Confirm_Your_Account")
                , L("EmailActivation_SubTitle"));
            

            emailTemplate.Replace("{EMAIL_BUTTON_LINK}", link);
            emailTemplate.Replace("{FOOTER_NOTIFICATION_TEXT}", L("DidNot_Reset_Password_Us"));
            emailTemplate.Replace("{CONTACT_SUPPORT}", L("Contact_Support"));
            emailTemplate.Replace("{EMAIL_BUTTON_NAME}", L("Email_Confirm_Button"));

            await ReplaceBodyAndSend(user.EmailAddress, L("EmailActivation_Subject"), emailTemplate, null);
        }

        /// <summary>
        /// Do not use
        /// Sends a password reset link to user's email.
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="link">Reset link</param>
        public async Task SendPasswordResetLinkAsync(User user, string link = null)
        {
            await CheckMailSettingsEmptyOrNull();

            if (user.PasswordResetCode.IsNullOrEmpty())
            {
                throw new Exception("PasswordResetCode should be set in order to send password reset link.");
            }

            var tenancyName = GetTenancyNameOrNull(user.TenantId);
            var emailTemplate = GetTitleAndSubTitle(user.TenantId, L("PasswordResetEmail_Title"), L("PasswordResetEmail_SubTitle"));
            var mailMessage = new StringBuilder();

            mailMessage.AppendLine("<b>" + L("NameSurname") + "</b>: " + user.Name + " " + user.Surname + "<br />");

            if (!tenancyName.IsNullOrEmpty())
            {
                mailMessage.AppendLine("<b>" + L("TenancyName") + "</b>: " + tenancyName + "<br />");
            }

            mailMessage.AppendLine("<b>" + L("UserName") + "</b>: " + user.UserName + "<br />");
            mailMessage.AppendLine("<b>" + L("ResetCode") + "</b>: " + user.PasswordResetCode + "<br />");

            if (!link.IsNullOrEmpty())
            {
                link = link.Replace("{userId}", user.Id.ToString());
                link = link.Replace("{resetCode}", Uri.EscapeDataString(user.PasswordResetCode));

                if (user.TenantId.HasValue)
                {
                    link = link.Replace("{tenantId}", user.TenantId.ToString());
                }

                link = EncryptQueryParameters(link);

                mailMessage.AppendLine("<br />");
                mailMessage.AppendLine(L("PasswordResetEmail_ClickTheLinkBelowToResetYourPassword") + "<br /><br />");
                mailMessage.AppendLine("<a style=\"" + _emailButtonStyle + "\" bg-color=\"" + _emailButtonColor + "\" href=\"" + link + "\">" + L("Reset") + "</a>");
                mailMessage.AppendLine("<br />");
                mailMessage.AppendLine("<br />");
                mailMessage.AppendLine("<br />");
                mailMessage.AppendLine("<span style=\"font-size: 9pt;\">" + L("EmailMessage_CopyTheLinkBelowToYourBrowser") + "</span><br />");
                mailMessage.AppendLine("<span style=\"font-size: 8pt;\">" + link + "</span>");
            }

            await ReplaceBodyAndSend(user.EmailAddress, L("PasswordResetEmail_Subject"), emailTemplate, mailMessage);
        }
        /// <summary>
        /// New functionality
        /// Sends a password reset link to user's email.
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="link">Reset link</param>
        public async Task SendPasswordResetLink(User user, string link = null)
        {
            await CheckMailSettingsEmptyOrNull();

            if (user.PasswordResetCode.IsNullOrEmpty())
            {
                throw new Exception("PasswordResetCode should be set in order to send password reset link.");
            }

            var emailTemplate = GetTemplate("layoutlovers.Net.Emailing.EmailTemplates.ResetPassword.resetPassword.html"
                , L("PasswordResetEmail_Title_LayoutLovers")
                , L("PasswordResetEmail_SubTitle_LayoutLovers"));

            if (!link.IsNullOrEmpty())
            {
                link = link.Replace("{userId}", user.Id.ToString());
                link = link.Replace("{resetCode}", Uri.EscapeDataString(user.PasswordResetCode));

                if (user.TenantId.HasValue)
                {
                    link = link.Replace("{tenantId}", user.TenantId.ToString());
                }

                link = EncryptQueryParameters(link);

                emailTemplate.Replace("{EMAIL_BUTTON_LINK}", link);
                emailTemplate.Replace("{FOOTER_NOTIFICATION_TEXT}", L("DidNot_Reset_Password"));
                emailTemplate.Replace("{CONTACT_SUPPORT}", L("Contact_Support"));
                emailTemplate.Replace("{EMAIL_BUTTON_NAME}", L("Reset_Password"));
            }

            await ReplaceBodyAndSend(user.EmailAddress, L("PasswordResetEmail_Subject"), emailTemplate, null);
        }

        public async Task TryToSendChatMessageMail(User user, string senderUsername, string senderTenancyName, ChatMessage chatMessage)
        {
            try
            {
                await CheckMailSettingsEmptyOrNull();

                var emailTemplate = GetTitleAndSubTitle(user.TenantId, L("NewChatMessageEmail_Title"), L("NewChatMessageEmail_SubTitle"));
                var mailMessage = new StringBuilder();

                mailMessage.AppendLine("<b>" + L("Sender") + "</b>: " + senderTenancyName + "/" + senderUsername + "<br />");
                mailMessage.AppendLine("<b>" + L("Time") + "</b>: " + chatMessage.CreationTime.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss") + " UTC<br />");
                mailMessage.AppendLine("<b>" + L("Message") + "</b>: " + chatMessage.Message + "<br />");
                mailMessage.AppendLine("<br />");

                await ReplaceBodyAndSend(user.EmailAddress, L("NewChatMessageEmail_Subject"), emailTemplate, mailMessage);
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message, exception);
            }
        }

        public async Task TryToSendSubscriptionExpireEmail(int tenantId, DateTime utcNow)
        {
            try
            {
                using (_unitOfWorkManager.Begin())
                {
                    using (_unitOfWorkManager.Current.SetTenantId(tenantId))
                    {
                        await CheckMailSettingsEmptyOrNull();

                        var tenantAdmin = await _userManager.GetAdminAsync();
                        if (tenantAdmin == null || string.IsNullOrEmpty(tenantAdmin.EmailAddress))
                        {
                            return;
                        }

                        var hostAdminLanguage = _settingManager.GetSettingValueForUser(LocalizationSettingNames.DefaultLanguage, tenantAdmin.TenantId, tenantAdmin.Id);
                        var culture = CultureHelper.GetCultureInfoByChecking(hostAdminLanguage);
                        var emailTemplate = GetTitleAndSubTitle(tenantId, L("SubscriptionExpire_Title"), L("SubscriptionExpire_SubTitle"));
                        var mailMessage = new StringBuilder();

                        mailMessage.AppendLine("<b>" + L("Message") + "</b>: " + L("SubscriptionExpire_Email_Body", culture, utcNow.ToString("yyyy-MM-dd") + " UTC") + "<br />");
                        mailMessage.AppendLine("<br />");

                        await ReplaceBodyAndSend(tenantAdmin.EmailAddress, L("SubscriptionExpire_Email_Subject"), emailTemplate, mailMessage);
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message, exception);
            }
        }

        public async Task TryToSendSubscriptionAssignedToAnotherEmail(int tenantId, DateTime utcNow, int expiringEditionId)
        {
            try
            {
                using (_unitOfWorkManager.Begin())
                {
                    using (_unitOfWorkManager.Current.SetTenantId(tenantId))
                    {
                        await CheckMailSettingsEmptyOrNull();

                        var tenantAdmin = await _userManager.GetAdminAsync();
                        if (tenantAdmin == null || string.IsNullOrEmpty(tenantAdmin.EmailAddress))
                        {
                            return;
                        }

                        var hostAdminLanguage = _settingManager.GetSettingValueForUser(LocalizationSettingNames.DefaultLanguage, tenantAdmin.TenantId, tenantAdmin.Id);
                        var culture = CultureHelper.GetCultureInfoByChecking(hostAdminLanguage);
                        var expringEdition = await _editionManager.GetByIdAsync(expiringEditionId);
                        var emailTemplate = GetTitleAndSubTitle(tenantId, L("SubscriptionExpire_Title"), L("SubscriptionExpire_SubTitle"));
                        var mailMessage = new StringBuilder();

                        mailMessage.AppendLine("<b>" + L("Message") + "</b>: " + L("SubscriptionAssignedToAnother_Email_Body", culture, expringEdition.DisplayName, utcNow.ToString("yyyy-MM-dd") + " UTC") + "<br />");
                        mailMessage.AppendLine("<br />");

                        await ReplaceBodyAndSend(tenantAdmin.EmailAddress, L("SubscriptionExpire_Email_Subject"), emailTemplate, mailMessage);
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message, exception);
            }
        }

        public async Task TryToSendFailedSubscriptionTerminationsEmail(List<string> failedTenancyNames, DateTime utcNow)
        {
            try
            {
                await CheckMailSettingsEmptyOrNull();

                var hostAdmin = await _userManager.GetAdminAsync();
                if (hostAdmin == null || string.IsNullOrEmpty(hostAdmin.EmailAddress))
                {
                    return;
                }

                var hostAdminLanguage = _settingManager.GetSettingValueForUser(LocalizationSettingNames.DefaultLanguage, hostAdmin.TenantId, hostAdmin.Id);
                var culture = CultureHelper.GetCultureInfoByChecking(hostAdminLanguage);
                var emailTemplate = GetTitleAndSubTitle(null, L("FailedSubscriptionTerminations_Title"), L("FailedSubscriptionTerminations_SubTitle"));
                var mailMessage = new StringBuilder();

                mailMessage.AppendLine("<b>" + L("Message") + "</b>: " + L("FailedSubscriptionTerminations_Email_Body", culture, string.Join(",", failedTenancyNames), utcNow.ToString("yyyy-MM-dd") + " UTC") + "<br />");
                mailMessage.AppendLine("<br />");

                await ReplaceBodyAndSend(hostAdmin.EmailAddress, L("FailedSubscriptionTerminations_Email_Subject"), emailTemplate, mailMessage);
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message, exception);
            }
        }

        public async Task TryToSendSubscriptionExpiringSoonEmail(int tenantId, DateTime dateToCheckRemainingDayCount)
        {
            try
            {
                using (_unitOfWorkManager.Begin())
                {
                    using (_unitOfWorkManager.Current.SetTenantId(tenantId))
                    {
                        await CheckMailSettingsEmptyOrNull();

                        var tenantAdmin = await _userManager.GetAdminAsync();
                        if (tenantAdmin == null || string.IsNullOrEmpty(tenantAdmin.EmailAddress))
                        {
                            return;
                        }

                        var tenantAdminLanguage = _settingManager.GetSettingValueForUser(LocalizationSettingNames.DefaultLanguage, tenantAdmin.TenantId, tenantAdmin.Id);
                        var culture = CultureHelper.GetCultureInfoByChecking(tenantAdminLanguage);

                        var emailTemplate = GetTitleAndSubTitle(null, L("SubscriptionExpiringSoon_Title"), L("SubscriptionExpiringSoon_SubTitle"));
                        var mailMessage = new StringBuilder();

                        mailMessage.AppendLine("<b>" + L("Message") + "</b>: " + L("SubscriptionExpiringSoon_Email_Body", culture, dateToCheckRemainingDayCount.ToString("yyyy-MM-dd") + " UTC") + "<br />");
                        mailMessage.AppendLine("<br />");

                        await ReplaceBodyAndSend(tenantAdmin.EmailAddress, L("SubscriptionExpiringSoon_Email_Subject"), emailTemplate, mailMessage);
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message, exception);
            }
        }
        public async Task SendNotificationAboutPurchaseProduct(User user, Purchase purchase, List<PurchaseItem> purchaseItems)
        {
            await CheckMailSettingsAnduserEmail(user);

            var emailTemplate = GetTemplate("layoutlovers.Net.Emailing.EmailTemplates.purchaseTemplate.html"
                , L("Email_Title_Thanks_Purchase")
                , L("EMAIL_SUB_TITLE_Thanks_Purchase", purchase.Id, purchase.CreationTime.ToString("dd MMMM yyyy")));


            var liItemTemplate = GetTemplate("layoutlovers.Net.Emailing.EmailTemplates.purchaseTemplateLi.html");
            var mailMessage = new StringBuilder();

            var items = purchaseItems.Select((value, i) => (value, i));

            foreach (var item in items)
            {
                var liItem = new StringBuilder(liItemTemplate.ToString());
                liItem = liItem.Replace("{EMAIL_TITLE_LI_INDEX}", $"{item.i + 1}");
                liItem = liItem.Replace("{EMAIL_TITLE_LI_NAME}", item.value.LayoutProduct.Name);
                liItem = liItem.Replace("{EMAIL_TITLE_LI_AMOUNT}", $"{item.value.Amount}");
                mailMessage.AppendLine(liItem.ToString());
            }

            await ReplaceBodyAndSend(user.EmailAddress, L("Thanks_Purchase_Subject"), emailTemplate, mailMessage);
        }

        public async Task SendNotificationUnsuccessfulPayment(User user)
        {
            await CheckMailSettingsAnduserEmail(user);

            var emailTemplate = GetTemplate("layoutlovers.Net.Emailing.EmailTemplates.purchaseTemplate.html"
                , L("Unsuccessful_Payment")
                , L("Unsuccessful_Payment_SubTitle"));

            emailTemplate.Replace("{EMAIL_BUTTON_NAME}", L("Unsuccessful_Payment_Button"));
            emailTemplate.Replace("{FOOTER_NOTIFICATION_TEXT}", L("Need_Help"));
            emailTemplate.Replace("{CONTACT_SUPPORT}", L("Contact_Support"));

            await ReplaceBodyAndSend(user.EmailAddress, L("Unsuccessful_Payment"), emailTemplate, null);
        }

        public async Task SendNotificationAboutNewProducts(User user, List<LayoutProductWithPreviewUrls> layoutProducts)
        {
            await CheckMailSettingsAnduserEmail(user);

            if (!layoutProducts.Any())
            {
                throw new UserFriendlyException("For mailing it is necessary to submit a list of products.");
            }            

            var productItems = layoutProducts.Select(f => f.LayoutProduct.Name).ToList();
            var subTitle = $"{string.Join(", ", productItems)}.";
            var emailTemplate = GetTemplate("layoutlovers.Net.Emailing.EmailTemplates.NewProducts.newProductsTemplate.html"
                , L("Email_Title_New_Product", user.Name)
                , subTitle);

            var liItemTemplate = GetTemplate("layoutlovers.Net.Emailing.EmailTemplates.NewProducts.newProductsTemplateLi.html");
            var mailMessage = new StringBuilder();

            foreach (var item in layoutProducts)
            {
                var liItem = new StringBuilder(liItemTemplate.ToString());
                liItem = liItem.Replace("{EMAIL_PRODUCT_IMAGE}", $"{item.PreviewUrls.FirstOrDefault()}");
                liItem = liItem.Replace("{EMAIL_PRODUCT_NAME}", item.LayoutProduct.Name);
                mailMessage.AppendLine(liItem.ToString());
            }

            emailTemplate.Replace("{EMAIL_BODY}", mailMessage.ToString());

            var emailFooter = GetTemplate("layoutlovers.Net.Emailing.EmailTemplates.footerTemplate.html");

            emailTemplate.Replace("{EMAIL_FOOTER}", emailFooter.ToString());

            await ReplaceBodyAndSend(user.EmailAddress, L("Thanks_NewProduct_Subject"), emailTemplate, mailMessage);
        }

        public async Task SendNotificationNewRegistrationSucceed(User user, SubscriptionPayment subscriptionPayment)
        {
            await CheckMailSettingsAnduserEmail(user);

            var emailTemplate = GetTemplate("layoutlovers.Net.Emailing.EmailTemplates.Membership.membershipOrder.html"
                , L("Thanks_For_Your_Order")
                , L("EMAIL_SUB_TITLE_Thanks_Purchase", subscriptionPayment.Id, subscriptionPayment.CreationTime.ToString("dd MMMM yyyy")));

            var editionDisplayName = subscriptionPayment.Edition.DisplayName;

            emailTemplate.Replace("{MEMBERSHIP_NAME}", editionDisplayName);
            emailTemplate.Replace("{AMOUNT_MEMBERSHIP}", $"${subscriptionPayment.Amount}");
            emailTemplate.Replace("{PERIOND_OF_SUBSCRIPTIONS}", L("PerMonth"));
            emailTemplate.Replace("{FOOTER_NOTIFICATION_TEXT}", L("Email_Membership_Footer"));
            emailTemplate.Replace("{EMAIL_BUTTON_NAME}", L("Email_Membership_Button"));

            await ReplaceBodyAndSend(user.EmailAddress, L("Thanks_For_Your_Order"), emailTemplate, null);
        }

        private StringBuilder GetTemplate(string filePath, string title = null, string subTitle = null)
        {
            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetTemplate(filePath));
            if (!title.IsNullOrWhiteSpace())
            {
                emailTemplate.Replace("{EMAIL_TITLE}", title);
            }
            if (!subTitle.IsNullOrWhiteSpace())
            {
                emailTemplate.Replace("{EMAIL_SUB_TITLE}", subTitle);
            }
            return emailTemplate;
        }

        private string GetTenancyNameOrNull(int? tenantId)
        {
            if (tenantId == null)
            {
                return null;
            }

            using (_unitOfWorkProvider.Current.SetTenantId(null))
            {
                return _tenantRepository.Get(tenantId.Value).TenancyName;
            }
        }

        private StringBuilder GetTitleAndSubTitle(int? tenantId, string title, string subTitle)
        {
            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetDefaultTemplate(tenantId));
            emailTemplate.Replace("{EMAIL_TITLE}", title);
            emailTemplate.Replace("{EMAIL_SUB_TITLE}", subTitle);

            return emailTemplate;
        }

        private async Task ReplaceBodyAndSend(string emailAddress, string subject, StringBuilder emailTemplate, StringBuilder mailMessage)
        {
            if (mailMessage != null && !mailMessage.ToString().IsNullOrWhiteSpace())
            {
                emailTemplate.Replace("{EMAIL_BODY}", mailMessage.ToString());
            }
            else
            {
                emailTemplate.Replace("{EMAIL_BODY}", "");
            }

            await _emailSender.SendAsync(new MailMessage
            {
                To = { emailAddress },
                Subject = subject,
                Body = emailTemplate.ToString(),
                IsBodyHtml = true
            });
        }

        private async Task CheckMailSettingsAnduserEmail(User user)
        {
            if (user == null)
            {
                throw new UserFriendlyException("The user cannot be null.");
            }

            await CheckMailSettingsEmptyOrNull();
            if (user.EmailConfirmationCode.IsNullOrEmpty())
            {
                throw new UserFriendlyException("EmailConfirmationCode should be set in order to send email activation link.");
            }
        }

        /// <summary>
        /// Returns link with encrypted parameters
        /// </summary>
        /// <param name="link"></param>
        /// <param name="encrptedParameterName"></param>
        /// <returns></returns>
        private string EncryptQueryParameters(string link, string encrptedParameterName = "c")
        {
            if (!link.Contains("?"))
            {
                return link;
            }

            var basePath = link.Substring(0, link.IndexOf('?'));
            var query = link.Substring(link.IndexOf('?')).TrimStart('?');

            return basePath + "?" + encrptedParameterName + "=" + HttpUtility.UrlEncode(SimpleStringCipher.Instance.Encrypt(query));
        }

        private async Task CheckMailSettingsEmptyOrNull()
        {
#if DEBUG
            return;
#endif
            if (
                (await _settingManager.GetSettingValueAsync(EmailSettingNames.DefaultFromAddress)).IsNullOrEmpty() ||
                (await _settingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Host)).IsNullOrEmpty()
            )
            {
                throw new UserFriendlyException(L("SMTPSettingsNotProvidedWarningText"));
            }

            if ((await _settingManager.GetSettingValueAsync<bool>(EmailSettingNames.Smtp.UseDefaultCredentials)))
            {
                return;
            }

            if (
                (await _settingManager.GetSettingValueAsync(EmailSettingNames.Smtp.UserName)).IsNullOrEmpty() ||
                (await _settingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Password)).IsNullOrEmpty()
            )
            {
                throw new UserFriendlyException(L("SMTPSettingsNotProvidedWarningText"));
            }
        }
    }
}
