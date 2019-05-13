namespace Njoy.Data
{
    public sealed class BusinessAddress : EntityBase
    {
        public Business Business { get; set; }
        public int BusinessId { get; set; }
        public District District { get; set; }
        public int DistrictId { get; set; }
        public string StreetAddress { get; set; }
        public string PostalCode { get; set; }
    }
}