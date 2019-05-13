using System.Collections.Generic;

namespace Njoy.Data
{
    public class Business : EntityBase
    {
        public string Name { get; set; }
        public BusinessAddress Address { get; set; }
        public virtual ICollection<BusinessMerchant> BusinessMerchants { get; set; }
    }
}