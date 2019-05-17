using System.Collections.Generic;

namespace Njoy.Admin
{
    public sealed class AddressPartsConfig
    {
        public IEnumerable<CreateAddressPartsFeature.Request.City> Cities { get; set; }
    }
}