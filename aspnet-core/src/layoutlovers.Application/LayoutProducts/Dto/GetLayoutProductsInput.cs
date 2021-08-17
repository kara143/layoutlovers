using Abp.Runtime.Validation;
using layoutlovers.Dto;
using System;
using System.Collections.Generic;

namespace layoutlovers.LayoutProducts.Dto
{
    public class GetLayoutProductsInput : PagedAndSortedInputDto, IShouldNormalize, IGetLayoutProductInput
    {
        public string Filter { get; set; }
        public Guid? CategoryId { get; set; }
        public SortFilter SortFilter { get; set; }
        public IEnumerable<Guid> FilterTagIds { get; set; }
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
