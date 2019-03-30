using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Njoy.Admin.IntegrationTests
{
    public class ServiceProviderHelper
    {
        private readonly ServiceProvider _serviceProvider;

        private ServiceProviderHelper(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public static ServiceProviderHelper CreateInstance<T>()
        {
            var services = new ServiceCollection();

            services.AddIdentity<AdminUser, AdminRole>()
                         .AddEntityFrameworkStores<AdminContext>()
                         .AddDefaultTokenProviders();

            services.AddDbContext<AdminContext>(options =>
            {
                options.UseInMemoryDatabase(databaseName: $"{nameof(T)}");
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

            return new ServiceProviderHelper(services.BuildServiceProvider());
        }

        public T Get<T>()
        {
            return _serviceProvider.GetService<T>();
        }
    }
}