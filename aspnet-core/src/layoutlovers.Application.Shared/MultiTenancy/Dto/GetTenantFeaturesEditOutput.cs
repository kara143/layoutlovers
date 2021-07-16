using System.Collections.Generic;
using Abp.Application.Services.Dto;
using layoutlovers.Editions.Dto;

namespace layoutlovers.MultiTenancy.Dto
{
    public class GetTenantFeaturesEditOutput
    {
        public List<NameValueDto> FeatureValues { get; set; }

        public List<FlatFeatureDto> Features { get; set; }
    }
}