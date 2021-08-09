using Abp.Application.Services.Dto;
using Abp.Linq.Extensions;
using layoutlovers.Products.Dto;
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

namespace layoutlovers.Products
{
    public class ProductsAppService : layoutloversAppServiceBase, IProductsAppService
    {
        private readonly IProductManager _productManager;
        private readonly IFilterTagManager _filterTagManager;
        private readonly IProductFilterTagManager _productFilterTagManager;
        private readonly IAmazonS3Manager _amazonS3Manager;
        public ProductsAppService(IProductManager productManager
            , IFilterTagManager filterTagManager
            , IProductFilterTagManager productFilterTagManager
            , IAmazonS3Manager amazonS3Manager)
        {
            _productFilterTagManager = productFilterTagManager;
            _filterTagManager = filterTagManager;
            _productManager = productManager;
            _amazonS3Manager = amazonS3Manager;
        }

        public async Task<ProductDto> Create(CreateProductDto input)
        {
            try
            {
                var product = ObjectMapper.Map<Product>(input);
                var entity = await _productManager.InsertAndGetEntityAsync(product);
                var productFilterTags = ObjectMapper.Map<IEnumerable<FilterTag>>(input.FilterTagDtos);

                var WithTagsWhatWasCreatedEarlier = productFilterTags.Where(f => f.Id != Guid.Empty)
                    .Select((f, index) => new ProductFilterTag
                    {
                        ProductId = product.Id,
                        FilterTagId = f.Id
                    }).ToArray();

                await _productFilterTagManager.InsertRange(WithTagsWhatWasCreatedEarlier);

                var withNewTags = productFilterTags.Where(f => f.Id == Guid.Empty)
                    .Select((f, index) => new ProductFilterTag
                    {
                        ProductId = product.Id,
                        FilterTag = f
                    }).ToArray();

                await _productFilterTagManager.InsertRange(withNewTags);

                var productDto = ObjectMapper.Map<ProductDto>(entity);
                return productDto;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while creating a product named: {input.Name}.", ex);
            }
        }

        public ProductDto GetByIdAndIncluding(Guid id)
        {
            var product = _productManager.GetByIdAndIncluding(id);
            var productDto = ObjectMapper.Map<ProductDto>(product);
            return productDto;
        }

        public async Task<ProductDto> GetById(Guid id)
        {
            var product = await _productManager.GetById(id);
            var productDto = ObjectMapper.Map<ProductDto>(product);
            return productDto;
        }

        public async Task<ProductDto> Update(UpdateProductDto input)
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
                   .Where(f => f.ProductId == input.Id)
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
                    ProductId = input.Id,
                    FilterTagId = tag.Id
                });

                await _productFilterTagManager.InsertRange(productFilterTagsToAdd);
            }

            var productFilterTags = tagsToCreateEntities.Select((tag, index) => new ProductFilterTag
            {
                ProductId = input.Id,
                FilterTag = tag
            }).ToArray();

            await _productFilterTagManager.InsertRange(productFilterTags);

            var product = ObjectMapper.Map<Product>(input);
            var entity = await _productManager.InsertOrUpdateAndGetEntityAsync(product);

            var productDto = ObjectMapper.Map<ProductDto>(entity);
            return productDto;
        }

        public async Task Delete(Guid id)
        {
            await _amazonS3Manager.DleteAllByProductId(id);

            await _productManager.DeleteAsync(id);
        }

        public async Task<PagedResultDto<ProductDto>> GetProducts(GetProductsInput input)
        {
            var query = _productManager.GetAll();

            query = Filtering(input, query);

            var productCount = query.Count();

            var products = await query
                .PageBy(input)
                .ToListAsync();

            var productListDtos = ObjectMapper.Map<List<ProductDto>>(products);

            return new PagedResultDto<ProductDto>(
                productCount,
                productListDtos
            );
        }

        private IQueryable<Product> Filtering(GetProductsInput input, IQueryable<Product> products)
        {
            var filter = input.Filter?.ToLower();

            var filterTagDtos = input.FilterTagIds.Distinct().ToList();

            var productsTest = products
                .WhereIf(input.CategoryId.HasValue, f => f.CategoryId == input.CategoryId)
                .WhereIf(!string.IsNullOrEmpty(filter), f => f.Name.ToLower().Contains(filter));

            //If there is at least one occurrence,
            //also sort by number of matches!
            if (filterTagDtos.IsNotEmpty())
            {
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
                    products = products.Where(f => f.ProductType == ProductType.Free);
                    break;
                case SortFilter.Premium:
                    products = products.Where(f => f.ProductType == ProductType.Premium);
                    break;
                default:
                    throw new Exception($"This type: {input.SortFilter} is not supported.");
            }

            if (!string.IsNullOrWhiteSpace(input.Sorting))
            {
                products = products
                    //.OrderBy(input.Sorting)
                    .OrderByDescending(f => f.CreationTime);
            }
            return products;
        }
    }
}
