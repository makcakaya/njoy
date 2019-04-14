using System.Collections.Generic;

namespace Njoy.Data
{
    public class Business : IPublicIdentity
    {
        public virtual ICollection<BusinessMerchant> BusinessMerchants { get; set; }
    }
}