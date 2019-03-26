using Microsoft.EntityFrameworkCore;

namespace Njoy.Admin
{
    public sealed class AdminContext : DbContext
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