using Abp.Runtime.Validation;
using layoutlovers.Dto;

namespace layoutlovers.Favorites.Models
{
    public class GetFavoritesInput : PagedAndSortedInputDto, IShouldNormalize
    {
        private string Filter { get; set; }
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "CreationTime";
            }

            Filter = Filter?.Trim();
        }
    }
}
