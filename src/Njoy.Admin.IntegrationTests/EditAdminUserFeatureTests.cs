using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Njoy.Admin.Features;
using System;
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
            Assert.Equal(request.FirstName, result.FirstName);
            Assert.Equal(request.LastName, result.LastName);
        }

        [Fact]
        public async void Can_Change_Password()
        {
            var userManager = GetUserManager();

            var user = new AdminUser
            {
                UserName = "TestUser",
                Email = "testuser@test.com"
            };
            var password = "Testpassword@123_";
            var newPassword = "newTestPassword@00";

            var identityResult = await userManager.CreateAsync(user, password);
            Assert.True(identityResult.Succeeded);
            Assert.NotEmpty(user.Id);

            var handler = new EditAdminUserFeature.Handler(userManager);
            var request = new EditAdminUserFeature.Request
            {
                Id = user.Id,
                FirstName = "First",
                LastName = "Last",
                CurrentPassword = password,
                NewPassword = newPassword,
                NewPasswordConfirm = newPassword
            };

            var result = await handler.Handle(request, new CancellationToken());
            Assert.NotNull(result);

            // To make sure that we changed the password, try to change it again.
            // If it succeeds, it means that previous operation was also successful.
            request.CurrentPassword = request.NewPassword;
            request.NewPassword = "ChangePassAgain@123";
            request.NewPasswordConfirm = "ChangePassAgain@123";

            result = await handler.Handle(request, new CancellationToken());
        }

        [Fact]
        public async void Change_Password_Throws_If_Current_Password_Not_Provided()
        {
            var userManager = GetUserManager();

            var user = new AdminUser
            {
                UserName = "TestUser",
                Email = "testuser@test.com"
            };
            var password = "Testpassword@123_";
            var newPassword = "newTestPassword@00";

            var identityResult = await userManager.CreateAsync(user, password);
            Assert.True(identityResult.Succeeded);
            Assert.NotEmpty(user.Id);

            var handler = new EditAdminUserFeature.Handler(userManager);
            var request = new EditAdminUserFeature.Request
            {
                Id = user.Id,
                FirstName = "First",
                LastName = "Last",
                NewPassword = newPassword,
                NewPasswordConfirm = newPassword
            };

            await Assert.ThrowsAsync<ArgumentException>(async () => await handler.Handle(request, new CancellationToken()));
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