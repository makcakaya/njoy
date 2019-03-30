using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Njoy.Admin
{
    public sealed class AdminContext : IdentityDbContext<AdminUser, AdminRole, string>
    {
        public DbSet<AdminUser> AdminUsers { get; set; }
        public DbSet<AdminRole> AdminRoles { get; set; }

        public AdminContext()
        {
        }

        public AdminContext(DbContextOptions<AdminContext> options) : base(options)
        {
        }
    }
}