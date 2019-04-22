using System.Collections.Generic;

namespace Njoy.Data
{
    public class Business : EntityBase
    {
        public virtual ICollection<BusinessMerchant> BusinessMerchants { get; set; }
    }
}