using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Njoy.Data;
using Njoy.Services;
using System.Threading;
using System.Threading.Tasks;

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

            services.AddIdentity<AppUser, AppRole>()
                         .AddEntityFrameworkStores<NjoyContext>()
                         .AddDefaultTokenProviders();

            services.AddDbContext<NjoyContext>(options =>
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

            services.AddScoped<IUserService, UserService>();

            var helper = new ServiceProviderHelper(services.BuildServiceProvider());

            var blocker = new ManualResetEvent(false);
            helper.CreateRolesAsync().ContinueWith((t) =>
            {
                blocker.Set();
            });
            blocker.WaitOne();

            return helper;
        }

        public T Get<T>()
        {
            return _serviceProvider.GetService<T>();
        }

        private async Task CreateRolesAsync()
        {
            // TODO: Create predefined roles on startup
            var roleManager = this.Get<RoleManager<AppRole>>();
            var roles = new string[] { AppRole.AdminRoot, AppRole.AdminStandart, AppRole.Sales };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    IdentityAssert.ThrowIfFailed(await roleManager.CreateAsync(new AppRole { Name = role }), $"Create {role} role");
                }
            }
        }
    }
}