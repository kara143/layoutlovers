using Abp.Runtime.Validation;
using layoutlovers.Dto;
using layoutlovers.LayoutProducts.Dto;
using System;
using System.Collections.Generic;

namespace layoutlovers.LayoutProducts.Models
{
    public class GetLayoutProductsInput : PagedAndSortedInputDto, IShouldNormalize, IGetLayoutProductInput
    {
        public bool IsFeaturedOnly { get; set; }
        public string Filter { get; set; }
        public Guid? CategoryId { get; set; }
        public SortFilter SortFilter { get; set; }
        public List<Guid> FilterTagIds { get; set; }
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
