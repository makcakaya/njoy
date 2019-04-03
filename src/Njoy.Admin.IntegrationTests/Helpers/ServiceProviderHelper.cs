using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Njoy.Admin.IntegrationTests
{
    public class ServiceProviderHelper
    {
        private static readonly string SqlConnectionString = "Server=localhost; Database=Njoy.Test; Trusted_Connection=true";
        private readonly ServiceProvider _serviceProvider;

        private ServiceProviderHelper(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public static ServiceProviderHelper CreateInstance<T>(bool useSqlServer = false)
        {
            var services = new ServiceCollection();

            services.AddIdentity<AdminUser, AdminRole>()
                         .AddEntityFrameworkStores<AdminContext>()
                         .AddDefaultTokenProviders();

            services.AddDbContext<AdminContext>(options =>
            {
                options.ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));
                var result = useSqlServer ? options.UseSqlServer(SqlConnectionString)
                    : options.UseInMemoryDatabase(databaseName: $"{nameof(T)}");
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