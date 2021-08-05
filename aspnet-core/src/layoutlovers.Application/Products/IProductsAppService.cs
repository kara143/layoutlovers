using Abp.Application.Services;
using Abp.Application.Services.Dto;
using layoutlovers.Products.Dto;
using System;
using System.Threading.Tasks;

namespace layoutlovers.Products
{
    public interface IProductsAppService : IApplicationService
    {
        Task<ProductDto> Create(CreateProductDto input);
        Task<ProductDto> GetById(Guid id);
        ProductDto GetByIdAndIncluding(Guid id);
        Task<PagedResultDto<ProductDto>> GetProducts(GetProductsInput input);
        Task<ProductDto> Update(UpdateProductDto input);
    }
}
