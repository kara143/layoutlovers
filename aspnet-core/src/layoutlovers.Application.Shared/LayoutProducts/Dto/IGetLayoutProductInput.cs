using Abp.Application.Services.Dto;
using System;

namespace layoutlovers.LayoutProducts.Dto
{
    public interface IGetLayoutProductInput: ISortedResultRequest
    {
        bool IsFeaturedOnly { get; set; }
        string Filter { get; set; }
        Guid? CategoryId { set; get; }
    }
}
