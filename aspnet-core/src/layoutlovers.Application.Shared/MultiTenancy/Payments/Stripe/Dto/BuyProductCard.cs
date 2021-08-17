using System;
using System.ComponentModel.DataAnnotations;

namespace layoutlovers.MultiTenancy.Payments.Stripe.Dto
{
    public class BuyProductCard: AddressStripeBase
    {
        [Required]
        public string Cvc { get; set; }
        [Required]
        public long ExpMonth { get; set; }
        [Required]
        public long ExpYear { get; set; }
        [Required]
        public string Number { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public Guid LayoutProductId { get; set; }
    }
}
