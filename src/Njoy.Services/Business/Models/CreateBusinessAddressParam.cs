namespace Njoy.Services
{
    public sealed class CreateBusinessAddressParam
    {
        public int BusinessId { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string District { get; set; }
        public string StreetAddress { get; set; }
    }
}