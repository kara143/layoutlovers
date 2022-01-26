using Abp.Application.Services;
using Abp.Application.Services.Dto;
using layoutlovers.LayoutProducts.Dto;
using layoutlovers.LayoutProducts.Models;
using layoutlovers.MultiTenancy.Payments.Stripe.Dto;
using layoutlovers.Purchases.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace layoutlovers.LayoutProducts
{
    public interface ILayoutProductsAppService : IApplicationService
    {
        Task<LayoutProductDto> Create(CreateLayoutProductDto input);
        Task<LayoutProductDto> GetById(Guid id);
        LayoutProductDto GetByIdAndIncluding(Guid id);
        Task<PagedResultDto<LayoutProductDto>> GetProducts(GetLayoutProductsInput input);
        Task<LayoutProductDto> Update(UpdateLayoutProductDto input);
        Task UpdateFeaturedOrder(IList<UpdateFeaturedOrderInput> updateFeaturedOrders);
        Task<PurchaseDto> BuyProduct(BuyProductCard buyProductCard);
        Task<PagedResultDto<LayoutProductDto>> GetShoppingHistoryProducts(GetShoppingHistory input);
    }
}
