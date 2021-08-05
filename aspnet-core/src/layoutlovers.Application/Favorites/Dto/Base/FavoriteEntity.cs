using Abp.Application.Services.Dto;
using System;

namespace layoutlovers.Favorites.Dto.Base
{
    public class FavoriteEntity : FavoriteBase, IEntityDto<Guid>
    {
        public Guid Id { get; set; }
    }
}
