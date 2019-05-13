namespace Njoy.Data
{
    public sealed class BusinessMerchant : EntityBase
    {
        public int MerchantId { get; set; }
        public Merchant Merchant { get; set; }
        public int BusinessId { get; set; }
        public Business Business { get; set; }
    }
}