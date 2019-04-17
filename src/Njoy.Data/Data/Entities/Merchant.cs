using System.Collections.Generic;

namespace Njoy.Data
{
    public class Merchant : EntityBase
    {
        public string UserId { get; set; }
        public virtual AppUser User { get; set; }
        public virtual ICollection<BusinessMerchant> BusinessMerchants { get; set; }
    }
}