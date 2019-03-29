using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Njoy.Admin.Features;
using System.Threading;
using Xunit;

namespace Njoy.Admin.IntegrationTests
{
    public sealed class EditAdminUserFeatureTests
    {
        [Fact]
        public async void Can_Add_Claims()
        {
            var userManager = GetUserManager();

            var user = new AdminUser
            {
                UserName = "TestUser",
                Email = "testuser@test.com"
            };
            var password = "Testpassword@123_";

            var identityResult = await userManager.CreateAsync(user, password);
            Assert.True(identityResult.Succeeded);
            Assert.NotEmpty(user.Id);

            var handler = new EditAdminUserFeature.Handler(userManager);
            var request = new EditAdminUserFeature.Request
            {
                Id = user.Id,
                FirstName = "First",
                LastName = "Last"
            };

            var result = await handler.Handle(request, new CancellationToken());
            Assert.NotNull(result);
        }

        private UserManager<AdminUser> GetUserManager()
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

            return services.BuildServiceProvider().GetService<UserManager<AdminUser>>();
        }
    }
}