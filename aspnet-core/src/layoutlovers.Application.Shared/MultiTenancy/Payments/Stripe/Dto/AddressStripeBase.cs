namespace layoutlovers.MultiTenancy.Payments.Stripe.Dto
{
    public class AddressStripeBase
    {
        public string AddressCity { get; set; }
        public string AddressCountry { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressState { get; set; }
        public string AddressZip { get; set; }
    }
}
