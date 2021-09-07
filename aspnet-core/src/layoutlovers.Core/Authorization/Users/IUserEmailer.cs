using System.Collections.Generic;
using System.Threading.Tasks;
using layoutlovers.Authorization.Users.EmailModels;
using layoutlovers.Chat;
using layoutlovers.PurchaseItems;
using layoutlovers.Purchases;

namespace layoutlovers.Authorization.Users
{
    public interface IUserEmailer
    {
        /// <summary>
        /// Send email activation link to user's email address.
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="link">Email activation link</param>
        /// <param name="plainPassword">
        /// Can be set to user's plain password to include it in the email.
        /// </param>
        Task SendEmailActivationLinkAsync(User user, string link, string plainPassword = null);
        Task SendNotificationAboutNewProducts(User user, List<LayoutProductWithPreviewUrls> layoutProducts);
        Task SendNotificationAboutPurchaseProduct(User user, Purchase purchase, List<PurchaseItem> purchaseItems);
        Task SendPasswordResetLink(User user, string link = null);

        /// <summary>
        /// Sends a password reset link to user's email.
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="link">Password reset link (optional)</param>
        Task SendPasswordResetLinkAsync(User user, string link = null);

        /// <summary>
        /// Sends an email for unread chat message to user's email.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="senderUsername"></param>
        /// <param name="senderTenancyName"></param>
        /// <param name="chatMessage"></param>
        Task TryToSendChatMessageMail(User user, string senderUsername, string senderTenancyName, ChatMessage chatMessage);
    }
}
