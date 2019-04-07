using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Njoy.Admin
{
    public sealed class AdminContext : IdentityDbContext<AdminUser, AdminRole, string>
    {
        public AdminContext()
        {
        }

        public AdminContext(DbContextOptions<AdminContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Identity User
            builder.Entity<AdminUser>().ToTable("AdminUsers");
            builder.Entity<IdentityUserClaim<string>>().ToTable("AdminUserClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("AdminUserLogins");
            builder.Entity<IdentityUserToken<string>>().ToTable("AdminUserTokens");

            // Identity Role
            builder.Entity<AdminRole>().ToTable("AdminRoles");
            builder.Entity<IdentityUserRole<string>>().ToTable("AdminUserRoles");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("AdminRoleClaims");
        }
    }
}