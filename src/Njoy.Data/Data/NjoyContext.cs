using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Njoy.Data
{
    public sealed class NjoyContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public NjoyContext()
        {
        }

        public NjoyContext(DbContextOptions<NjoyContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyIdentityConfigurations();
            builder.ApplyConfiguration(new MerchantConfig());
            builder.ApplyConfiguration(new BusinessConfig());
            builder.ApplyConfiguration(new BusinessMerchantConfig());
        }
    }
}