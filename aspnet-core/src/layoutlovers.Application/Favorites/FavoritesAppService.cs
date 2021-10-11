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
        public FavoritesAppService(IRepository<Favorite, Guid> repository) : base(repository)
        {
        }

        public async Task<PagedResultDto<FavoriteDto>> GetAllOfCurrentUserAsync(GetFavoritesInput input)
        {
            try
            {
                var userId = AbpSession.GetUserId();

                var query = Repository.GetAll()
                    .Where(f => f.UserId == userId);

                var favoriteCount = query.Count();

                var favorites = await query
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                var favoriteListDto = ObjectMapper.Map<List<FavoriteDto>>(favorites);

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
