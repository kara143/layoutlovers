using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.UI;
using layoutlovers.Authorization.Users;
using layoutlovers.Editions;
using layoutlovers.Extensions;
using layoutlovers.MultiTenancy.Accounting.Dto;
using layoutlovers.MultiTenancy.Payments.Stripe;
using layoutlovers.MultiTenancy.Payments.Stripe.Dto;
using layoutlovers.PurchaseItems;
using layoutlovers.Purchases;
using layoutlovers.Purchases.Dto;
using layoutlovers.ShoppingCarts.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace layoutlovers.ShoppingCarts
{
    public class ShoppingCartsAppService : CrudAppServiceBase<ShoppingCart
            , ShoppingCartDto
            , Guid
            , PagedAndSortedResultRequestDto
            , CreateShoppingCartDto
            , UpdateShoppingCartDto>, IShoppingCartsAppService
    {
        private readonly IStripePaymentAppService _stripePaymentAppService;
        private readonly IPurchaseManager _purchaseManager;
        private readonly IShoppingCartManager _shoppingCartManager;
        private readonly IPurchaseItemManager _purchaseItemManager;
        private readonly EditionManager _editionManager;
        private readonly IUserEmailer _userEmailer;

        public ShoppingCartsAppService(IRepository<ShoppingCart, Guid> repository
            , IStripePaymentAppService stripePaymentAppService
            , IPurchaseManager purchaseManager
            , IShoppingCartManager shoppingCartManager
            , IPurchaseItemManager purchaseItemManager
            , EditionManager editionManager
            , IUserEmailer userEmailer
            ) : base(repository)
        {
            _stripePaymentAppService = stripePaymentAppService;
            _purchaseManager = purchaseManager;
            _shoppingCartManager = shoppingCartManager;
            _purchaseItemManager = purchaseItemManager;
            _editionManager = editionManager;
            _userEmailer = userEmailer;
        }

        public async Task<GetShoppingCartDto> GetCurrentUserShoppingCart()
        {
            var user = await GetCurrentUserAsync();
            var shoppingCartItems = _shoppingCartManager.GetShoppingCartItemsByUserId(user.Id)
                .ToList();

            var totalAmount = shoppingCartItems.Select(f => f.LayoutProduct)
                .Select(f => f.Amount)
                .Sum();

            var getShoppingCartDto = new GetShoppingCartDto
            {
                ShoppingCartDtos = ObjectMapper.Map<IEnumerable<ShoppingCartDto>>(shoppingCartItems),
                TotalCount = shoppingCartItems.Count,
                TotalAmount = totalAmount
            };

            return getShoppingCartDto;
        }

        public async Task<PurchaseDto> BuyShoppingCart(BuyProduct buyProduct)
        {
            var user = await GetCurrentUserAsync();

            var shoppingCartItems = _shoppingCartManager.GetShoppingCartItemsByUserId(user.Id).ToList();

            if (!shoppingCartItems.Any())
            {
                throw new UserFriendlyException("The payment could not be processed because the shopping cart is empty.");
            }

            var products = shoppingCartItems.Select(f => f.LayoutProduct)
                .ToList();

            var totalAmount = products
                 .Select(f => f.Amount)
                 .Sum();

            var paymentCardDto = ObjectMapper.Map<PaymentCardDto>(buyProduct);

            paymentCardDto.Email = user.EmailAddress;
            paymentCardDto.Name = user.FullName;
            paymentCardDto.Amount = totalAmount;
            paymentCardDto.Currency = "USD";

            var charge = await _stripePaymentAppService.TryBuyProduct(paymentCardDto);
            var purchase = await _purchaseManager.InsertPurchaseAsync(charge, products, user.Id);

            //TODO: improve
            foreach (var item in shoppingCartItems)
            {
                await Repository.DeleteAsync(f => f.Id == item.Id);
            }

            var purchaseDto = ObjectMapper.Map<PurchaseDto>(purchase);
            var purchaseItems = purchase.PurchaseItems.ToList();
            await _userEmailer.SendNotificationAboutPurchaseProduct(user, purchase, purchaseItems);

            return purchaseDto;
        }

        public override async Task<ShoppingCartDto> CreateAsync(CreateShoppingCartDto input)
        {
            var currentUser = await GetCurrentUserAsync();
            var userId = currentUser.Id;

            if (userId != input.UserId)
            {
                throw new UserFriendlyException($"The passed id {input.UserId} is not the logged in user id {userId}");
            }

            var editionId = await GetEditionId();
            //Only a free subscription is eligible to buy a product or pay for a basket.

            var isFree = await _editionManager.IsFree(editionId);
            if (!isFree)
            {
                throw new UserFriendlyException($"Add to shopping cart can only be purchased with a {EditionManager.DefaultEditionName} subscription.");
            }

            var shoppingCartItem = _shoppingCartManager.GetAll().FirstOrDefault(f => f.UserId == userId
                                                        && f.LayoutProductId == input.LayoutProductId);

            //Check if there is a similar entry in the basket of this user
            if (shoppingCartItem.IsNotNull())
            {
                throw new UserFriendlyException($"There is already an entry in the shopping cart with the given user id {userId}" +
                    $" and product id {input.LayoutProductId} .");
            }
            //Check if this product was purchased.
            //Since a product already purchased cannot be added to the basket.
            var isBought = await _purchaseItemManager.IsBought(input.LayoutProductId, userId);
            if (isBought)
            {
                throw new UserFriendlyException($"The product with ID {input.LayoutProductId} has already been purchased and therefore cannot be added to the basket.");
            }

            return await base.CreateAsync(input);
        }
    }
}
