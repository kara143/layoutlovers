using System;
using System.ComponentModel.DataAnnotations;

namespace layoutlovers.MultiTenancy.Payments.Stripe.Dto
{
    public class BuyProductCard: BuyProduct
    {
        [Required]
        public Guid LayoutProductId { get; set; }
    }
}
