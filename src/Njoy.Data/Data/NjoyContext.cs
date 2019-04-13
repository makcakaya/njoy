using Microsoft.AspNetCore.Identity;
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

            // Identity User
            builder.Entity<AppUser>().ToTable("AppUsers");
            builder.Entity<IdentityUserClaim<string>>().ToTable("AppUserClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("AppUserLogins");
            builder.Entity<IdentityUserToken<string>>().ToTable("AppUserTokens");

            // Identity Role
            builder.Entity<AppRole>().ToTable("AppRoles");
            builder.Entity<IdentityUserRole<string>>().ToTable("AppUserRoles");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("AppRoleClaims");
        }
    }
}