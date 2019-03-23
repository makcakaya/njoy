using Microsoft.EntityFrameworkCore;

namespace Njoy.Admin
{
    public sealed class AdminContext : DbContext
    {
        public AdminContext(DbContextOptions<AdminContext> options) : base(options)
        {
        }
    }
}