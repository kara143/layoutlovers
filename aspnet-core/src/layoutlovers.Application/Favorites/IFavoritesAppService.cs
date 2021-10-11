using Abp.Application.Services.Dto;
using layoutlovers.Favorites.Dto;
using layoutlovers.Favorites.Models;
using System;
using System.Threading.Tasks;

namespace layoutlovers.Favorites
{
    public interface IFavoritesAppService : ICrudAppServiceBase<FavoriteDto
        , Guid
        , PagedAndSortedResultRequestDto
        , CreateFavoriteDto
        , UpdateFavoriteDto>
    {
        Task<PagedResultDto<FavoriteDto>> GetAllOfCurrentUserAsync(GetFavoritesInput input);
    }
}
