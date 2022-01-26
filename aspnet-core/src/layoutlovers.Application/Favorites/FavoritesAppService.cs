using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.Runtime.Session;
using layoutlovers.Favorites.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Abp.UI;
using Abp.Authorization;
using layoutlovers.Favorites.Models;
using layoutlovers.Amazon;

namespace layoutlovers.Favorites
{
    [AbpAuthorize]
    public class FavoritesAppService :
        CrudAppServiceBase<Favorite
            , FavoriteDto
            , Guid
            , PagedAndSortedResultRequestDto
            , CreateFavoriteDto
            , UpdateFavoriteDto>, IFavoritesAppService
    {
        private readonly AmazonS3Configuration _amazonS3Configuration;
        public FavoritesAppService(IRepository<Favorite, Guid> repository, AmazonS3Configuration amazonS3Configuration) : base(repository)
        {
            _amazonS3Configuration = amazonS3Configuration;
        }

        public async Task<PagedResultDto<FavoriteDto>> GetAllOfCurrentUserAsync(GetFavoritesInput input)
        {
            try
            {
                var userId = AbpSession.GetUserId();

                var query = Repository.GetAllIncluding(f => f.LayoutProduct, f => f.LayoutProduct.AmazonS3Files)
                    .Where(f => f.UserId == userId);

                var favoriteCount = query.Count();

                var favorites = await query
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                var favoriteListDto = ObjectMapper.Map<List<FavoriteDto>>(favorites);
                favoriteListDto.Select((favorite, i) =>
                {
                    var thumbnail = favorite.Thumbnail;
                    if (thumbnail != null)
                    {
                        thumbnail.PreviewUrl = _amazonS3Configuration.GetPreviewUrl(thumbnail.LayoutProductId, thumbnail.Name);
                    }
                    return thumbnail;
                }).ToList();

                return new PagedResultDto<FavoriteDto>(
                    favoriteCount,
                    favoriteListDto
                );
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }            
        }
    }
}
