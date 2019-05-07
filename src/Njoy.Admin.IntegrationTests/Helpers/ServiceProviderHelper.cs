using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Njoy.Data;
using Njoy.Services;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace Njoy.Admin.IntegrationTests
{
    public class ServiceProviderHelper
    {
        //private static readonly string SqlConnectionString = "Server=localhost; Database=Njoy.Test; Trusted_Connection=true";
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

            var config = ConfigurationHelper.Get();
            services.AddDbContext<NjoyContext>(options =>
            {
                options.ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));
                var result = useSqlServer ? options.UseSqlServer(config.GetConnectionString("DefaultConnection"))
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
            services.AddScoped<IMerchantService, MerchantService>();
            services.CustomAddContext(config);
            services.CustomAddIdentity(config.GetSection("JwtSettings").Get<JwtSettings>());
            var container = services.CustomAddSimpleInjector();

            var helper = new ServiceProviderHelper(services.BuildServiceProvider());
            container.RegisterMediator();
            container.AutoCrossWireAspNetComponents(helper._serviceProvider);
            SimpleInjectorExtensions.RegisterConfigurations(container, config);

            RunCustomStartupTasks(container);

            return helper;
        }

        private static void RunCustomStartupTasks(Container container)
        {
            using (AsyncScopedLifestyle.BeginScope(container))
            {
                var mediator = container.GetService<IMediator>();
                CustomStartupTasksExtensions.Run(mediator);
            }
        }

        public T Get<T>()
        {
            return _serviceProvider.GetService<T>();
        }
    }
}