using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Njoy.Data
{
    public static class IdentityConfigurationExtensions
    {
        public static void ApplyIdentityConfigurations(this ModelBuilder builder)
        {
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