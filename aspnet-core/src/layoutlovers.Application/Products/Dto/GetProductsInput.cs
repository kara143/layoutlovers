using Abp.Runtime.Validation;
using layoutlovers.Dto;
using System;
using System.Collections.Generic;

namespace layoutlovers.Products.Dto
{
    public class GetProductsInput : PagedAndSortedInputDto, IShouldNormalize, IGetProductInput
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
