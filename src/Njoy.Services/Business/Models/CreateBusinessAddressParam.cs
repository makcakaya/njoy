namespace Njoy.Services
{
    public sealed class CreateBusinessAddressParam
    {
        public int BusinessId { get; set; }
        public int DistrictId { get; set; }
        public string PostalCode { get; set; }
        public string StreetAddress { get; set; }
    }
}