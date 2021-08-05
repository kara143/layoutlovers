using Abp.Application.Services.Dto;
using System;

namespace layoutlovers.Products.Dto
{
    public interface IGetProductInput: ISortedResultRequest
    {
        string Filter { get; set; }
        Guid? CategoryId { set; get; }
    }
}
