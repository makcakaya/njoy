using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Njoy.Admin
{
    public static class ContextExtensions
    {
        public static void CustomAddContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AdminContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }
    }
}