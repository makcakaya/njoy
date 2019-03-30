using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Njoy.Admin.IntegrationTests
{
    public class ServiceProviderHelper
    {
        private readonly ServiceProvider _serviceProvider;

        public ServiceProviderHelper()
        {
            var services = new ServiceCollection();

            services.AddIdentity<AdminUser, AdminRole>()
                         .AddEntityFrameworkStores<AdminContext>()
                         .AddDefaultTokenProviders();

            services.AddDbContext<AdminContext>(options =>
            {
                options.UseInMemoryDatabase(databaseName: $"{nameof(EditAdminUserFeatureTests)}");
            });

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
                options.User.RequireUniqueEmail = false;
            });

            _serviceProvider = services.BuildServiceProvider();
        }

        public T Get<T>()
        {
            return _serviceProvider.GetService<T>();
        }
    }
}