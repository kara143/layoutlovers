using layoutlovers.MultiTenancy.Payments.Stripe.Dto;
using System.ComponentModel.DataAnnotations;

namespace layoutlovers.MultiTenancy.Accounting.Dto
{
    public class PaymentCardDto: AddressStripeBase
    {
        [Required]
        public string Currency { get; set; }
        [Required]
        public string Cvc { get; set; }
        [Required]
        public long ExpMonth { get; set; }
        [Required]
        public long ExpYear { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Number { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
