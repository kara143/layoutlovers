using System;

namespace layoutlovers.LayoutProducts.Models
{
    public class UpdateFeaturedOrderInput
    {
        public Guid LayoutProductId { get; set; }
        public int FeaturedOrder { get; set; }
    }
}
