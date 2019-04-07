using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Njoy.Data
{
    public sealed class NjoyContext : IdentityDbContext<AppUser, AppRole, int>
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

            // Identity User
            builder.Entity<AppUser>().ToTable("AppUsers");
            builder.Entity<IdentityUserClaim<int>>().ToTable("AppUserClaims");
            builder.Entity<IdentityUserLogin<int>>().ToTable("AppUserLogins");
            builder.Entity<IdentityUserToken<int>>().ToTable("AppUserTokens");

            // Identity Role
            builder.Entity<AppRole>().ToTable("AppRoles");
            builder.Entity<IdentityUserRole<int>>().ToTable("AppUserRoles");
            builder.Entity<IdentityRoleClaim<int>>().ToTable("AppRoleClaims");
        }
    }
}