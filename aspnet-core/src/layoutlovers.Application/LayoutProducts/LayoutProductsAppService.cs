using Abp.Application.Services.Dto;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using layoutlovers.FilterTags;
using layoutlovers.ProductFilterTags;
using Abp.Collections.Extensions;
using layoutlovers.Extensions;
using layoutlovers.Amazon;
using layoutlovers.MultiTenancy.Accounting.Dto;
using layoutlovers.MultiTenancy.Payments.Stripe.Dto;
using layoutlovers.MultiTenancy.Payments.Stripe;
using Abp.Authorization;
using layoutlovers.LayoutProducts.Dto;
using layoutlovers.Purchases;
using layoutlovers.Purchases.Dto;
using layoutlovers.PurchaseItems;
using Abp.UI;

namespace layoutlovers.LayoutProducts
{
    [AbpAuthorize]
    public class LayoutProductsAppService : layoutloversAppServiceBase, ILayoutProductsAppService
    {
        private readonly ILayoutProductManager _layoutProductManager;
        private readonly IFilterTagManager _filterTagManager;
        private readonly IProductFilterTagManager _productFilterTagManager;
        private readonly IAmazonS3Manager _amazonS3Manager;
        private readonly IStripePaymentAppService _stripePaymentAppService;
        private readonly IPurchaseManager _purchaseManager;
        private readonly IPurchaseItemManager _purchaseItemManager;
        public LayoutProductsAppService(ILayoutProductManager layoutProductManager
            , IFilterTagManager filterTagManager
            , IProductFilterTagManager productFilterTagManager
            , IAmazonS3Manager amazonS3Manager
            , IStripePaymentAppService stripePaymentAppService
            , IPurchaseManager purchaseManager
            , IPurchaseItemManager purchaseItemManager
            )
        {
            _productFilterTagManager = productFilterTagManager;
            _filterTagManager = filterTagManager;
            _layoutProductManager = layoutProductManager;
            _amazonS3Manager = amazonS3Manager;
            _stripePaymentAppService = stripePaymentAppService;
            _purchaseManager = purchaseManager;
            _purchaseItemManager = purchaseItemManager;
        }

        public async Task<LayoutProductDto> Create(CreateLayoutProductDto input)
        {
            if (input.LayoutProductType.IsFree() && input.Amount != 0)
            {
                throw new UserFriendlyException("There cannot be an amount in a free product.");
            }

            if (!input.LayoutProductType.IsFree() && input.Amount == 0)
            {
                throw new UserFriendlyException("There was no amount specified in the product.");
            }

            try
            {
                var product = ObjectMapper.Map<LayoutProduct>(input);
                var entity = await _layoutProductManager.InsertAndGetEntityAsync(product);
                var productFilterTags = ObjectMapper.Map<IEnumerable<FilterTag>>(input.FilterTagDtos);

                var WithTagsWhatWasCreatedEarlier = productFilterTags.Where(f => f.Id != Guid.Empty)
                    .Select((f, index) => new ProductFilterTag
                    {
                        LayoutProductId = product.Id,
                        FilterTagId = f.Id
                    }).ToArray();

                await _productFilterTagManager.InsertRange(WithTagsWhatWasCreatedEarlier);

                var withNewTags = productFilterTags.Where(f => f.Id == Guid.Empty)
                    .Select((f, index) => new ProductFilterTag
                    {
                        LayoutProductId = product.Id,
                        FilterTag = f
                    }).ToArray();

                await _productFilterTagManager.InsertRange(withNewTags);

                var productDto = ObjectMapper.Map<LayoutProductDto>(entity);
                return productDto;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException($"An error occurred while creating a product named: {input.Name}.", ex);
            }
        }

        public LayoutProductDto GetByIdAndIncluding(Guid id)
        {
            var product = _layoutProductManager.GetByIdAndIncluding(id);
            var productDto = ObjectMapper.Map<LayoutProductDto>(product);
            return productDto;
        }

        public async Task<LayoutProductDto> GetById(Guid id)
        {
            var product = await _layoutProductManager.GetById(id);
            var productDto = ObjectMapper.Map<LayoutProductDto>(product);
            return productDto;
        }

        public async Task<LayoutProductDto> Update(UpdateLayoutProductDto input)
        {
            //Tags what have curent product
            var curentProductTags = _productFilterTagManager.GetFilterTagByProductId(input.Id).ToList();

            //Tags that will definitely be updated as they have ID.
            var tagsWithId = input.FilterTagDtos.Where(f => f.Id != Guid.Empty).ToList();
            var tagsWithIdEntities = ObjectMapper.Map<List<FilterTag>>(tagsWithId);

            //Tags that do not have an ID but may duplicate the name.
            var tagsWithoutId = input.FilterTagDtos.Where(f => f.Id == Guid.Empty).ToList();

            //Tags that were created earlier and were found by keyword!
            var tagsCreatedEarlier = _filterTagManager.GetAll()
                .ToList()
                .FilterIf(tagsWithoutId.IsNotEmpty(), f => tagsWithoutId.Any(t => t.Name == f.Name));

            //New tags what we will add to the database!
            var tagsToCreate = tagsWithoutId.WhereIf(tagsCreatedEarlier.IsNotEmpty(), f => tagsCreatedEarlier.Any(t => !t.Name.Equals(f.Name)));
            var tagsToCreateEntities = ObjectMapper.Map<IEnumerable<FilterTag>>(tagsToCreate);

            //Tag that will be removed from the product link and tag!
            var deletedTags = curentProductTags.WhereIf(tagsWithId.IsNotEmpty(), f => !tagsWithId.Any(a => a.Id == f.Id))
                .ToList();

            if (deletedTags.IsNotEmpty())
            {
                var productTags = _productFilterTagManager
                   .GetAll()
                   .Where(f => f.LayoutProductId == input.Id)
                   .ToList()
                   .Where(f => deletedTags.Any(tag => tag.Id == f.FilterTagId));

                foreach (var tag in productTags)
                {
                    await _productFilterTagManager.DeleteAsync(tag.Id);
                }
            }

            //Add existing tags to the product!
            if (tagsCreatedEarlier.IsNotEmpty())
            {
                var productFilterTagsToAdd = tagsCreatedEarlier.Select((tag, i) => new ProductFilterTag
                {
                    LayoutProductId = input.Id,
                    FilterTagId = tag.Id
                });

                await _productFilterTagManager.InsertRange(productFilterTagsToAdd);
            }

            var productFilterTags = tagsToCreateEntities.Select((tag, index) => new ProductFilterTag
            {
                LayoutProductId = input.Id,
                FilterTag = tag
            }).ToArray();

            await _productFilterTagManager.InsertRange(productFilterTags);

            var product = ObjectMapper.Map<LayoutProduct>(input);
            var entity = await _layoutProductManager.InsertOrUpdateAndGetEntityAsync(product);

            var productDto = ObjectMapper.Map<LayoutProductDto>(entity);
            return productDto;
        }

        public async Task Delete(Guid id)
        {
            await _amazonS3Manager.DleteAllByProductId(id);

            await _layoutProductManager.DeleteAsync(id);
        }

        public async Task<PagedResultDto<LayoutProductDto>> GetProducts(GetLayoutProductsInput input)
        {
            var query = _layoutProductManager.GetAll();
            query = Filtering(input, query);

            var productCount = query.Count();

            var products = await query
                .PageBy(input)
                .ToListAsync();

            var productListDtos = ObjectMapper.Map<List<LayoutProductDto>>(products);

            var user = await GetCurrentUserAsync();

            //Purchased products by the current user
            var purchasedProductIds = _purchaseItemManager.GetAllPurchasedProductByUserId(user.Id)
                .Select(f => f.Id)
                .ToList();

            var result = productListDtos.Select((product, i) =>
            {
                product.IsPurchased = purchasedProductIds.Any(i => i == product.Id);
                return product;
            }).ToList();

            return new PagedResultDto<LayoutProductDto>(
                productCount,
                result
            );
        }

        private IQueryable<LayoutProduct> Filtering(GetLayoutProductsInput input, IQueryable<LayoutProduct> products)
        {
            var filter = input.Filter?.ToLower();

            var productsTest = products
                .WhereIf(input.CategoryId.HasValue, f => f.CategoryId == input.CategoryId)
                .WhereIf(!string.IsNullOrEmpty(filter), f => f.Name.ToLower().Contains(filter));

            //If there is at least one occurrence,
            //also sort by number of matches!
            if (input.FilterTagIds.IsNotEmpty())
            {
                var filterTagDtos = input.FilterTagIds.Distinct().ToList();
                var filterTagCount = filterTagDtos.Count();

                products = productsTest
                    .Select(product => new
                    {
                        product = product,
                        matchTagCount = product.ProductFilterTags
                            .Where(prodTag => filterTagDtos.Any(f => f == prodTag.FilterTagId))
                            .Count()
                    })
                    .Where(f => f.matchTagCount == filterTagCount)
                    .Select(f => f.product);
            }

            switch (input.SortFilter)
            {
                case SortFilter.Default:
                    //ignore
                    break;
                case SortFilter.High:
                    products = products.OrderBy(f => f.Amount);
                    break;
                case SortFilter.Low:
                    products = products.OrderByDescending(f => f.Amount);
                    break;
                case SortFilter.Free:
                    products = products.Where(f => f.LayoutProductType == LayoutProductType.Free);
                    break;
                case SortFilter.Premium:
                    products = products.Where(f => f.LayoutProductType == LayoutProductType.Premium);
                    break;
                default:
                    throw new UserFriendlyException($"This type: {input.SortFilter} is not supported.");
            }

            if (!string.IsNullOrWhiteSpace(input.Sorting))
            {
                products = products
                    //.OrderBy(input.Sorting)
                    .OrderByDescending(f => f.CreationTime);
            }
            return products;
        }

        public async Task<PurchaseDto> BuyProduct(BuyProductCard buyProductCard)
        {
            var productId = buyProductCard.LayoutProductId;

            var product = await _layoutProductManager.GetById(productId);
            if (product == null)
            {
                throw new UserFriendlyException($"Product with id {productId} not found!");
            }

            if (product.LayoutProductType == LayoutProductType.Free)
            {
                throw new UserFriendlyException($"Product ID {product.Id} is free of charge and " +
                    $"therefore cannot be purchased.");
            }

            var user = await GetCurrentUserAsync();

            var isBought = await _purchaseItemManager.IsBought(productId, user.Id);
            //Before making a payment, check if this product has already been purchased by the current user.
            if (isBought)
            {
                throw new UserFriendlyException($"The product with id {productId} has already been purchased.");
            }

            var paymentCardDto = ObjectMapper.Map<PaymentCardDto>(buyProductCard);

            paymentCardDto.Email = user.EmailAddress;
            paymentCardDto.Name = user.FullName;
            paymentCardDto.Amount = product.Amount;
            paymentCardDto.Currency = "USD";

            var charge = await _stripePaymentAppService.TryBuyProduct(paymentCardDto);

            var purchase = await _purchaseManager.InsertPurchaseAsync(charge, new List<LayoutProduct> { product }, user.Id);
            var purchaseDto = ObjectMapper.Map<PurchaseDto>(purchase);
            return purchaseDto;
        }
    }
}
