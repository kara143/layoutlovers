using Abp.Runtime.Validation;
using layoutlovers.Dto;

namespace layoutlovers.LayoutProducts.Models
{
    public class GetShoppingHistory : PagedAndSortedInputDto, IShouldNormalize
    {
        public ShoppingHistoryType ShoppingHistoryType { get; set; }
        public string Filter { get; set; }

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
