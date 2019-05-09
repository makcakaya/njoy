using System;

namespace Njoy.Data
{
    public class BusinessSubscription : EntityBase
    {
        public virtual Business Business { get; set; }
        public int BusinessId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public BusinessSubscriptionState State { get; set; }
    }
}