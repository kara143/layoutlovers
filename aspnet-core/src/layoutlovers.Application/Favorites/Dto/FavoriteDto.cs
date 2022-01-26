using Abp.AutoMapper;
using layoutlovers.Favorites.Dto.Base;
using layoutlovers.Files;
using layoutlovers.Files.Dto;
using System.Collections.Generic;

namespace layoutlovers.Favorites.Dto
{
    [AutoMap(typeof(Favorite))]
    public class FavoriteDto: FavoriteEntity
    {
        public string ProductName { get; set; }
        public S3ImageDto Thumbnail { get; set; }
        public IEnumerable<FileType> FileExtension { get; set; }

    }
}
