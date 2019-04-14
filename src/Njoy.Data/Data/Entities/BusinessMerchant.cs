namespace Njoy.Data
{
    public sealed class BusinessMerchant
    {
        public int Id { get; set; }
        public int MerchantId { get; set; }
        public Merchant Merchant { get; set; }
        public int BusinessId { get; set; }
        public Business Business { get; set; }
    }
}