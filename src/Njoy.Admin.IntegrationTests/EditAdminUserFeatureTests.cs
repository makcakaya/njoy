using Microsoft.AspNetCore.Identity;
using Njoy.Admin.Features;
using Njoy.Data;
using Njoy.Services;
using System;
using System.Security.Claims;
using System.Threading;
using Xunit;

namespace Njoy.Admin.IntegrationTests
{
    public sealed class EditAdminUserFeatureTests
    {
        [Fact]
        public async void Can_Add_Claims()
        {
            var serviceProvider = ServiceProviderHelper.CreateInstance<EditAdminUserFeatureTests>();
            var userManager = serviceProvider.Get<UserManager<AppUser>>();

            var user = new AppUser
            {
                UserName = "TestUser",
                Email = "testuser@test.com"
            };
            var password = "Testpassword@123_";

            var identityResult = await userManager.CreateAsync(user, password);
            Assert.True(identityResult.Succeeded);
            Assert.NotEmpty(user.Id);

            var request = new EditAdminUserFeature.Request
            {
                Id = user.Id,
                FirstName = "First",
                LastName = "Last"
            };

            var handler = GetHandler(serviceProvider);
            await handler.Handle(request, new CancellationToken());

            user = await userManager.FindByIdAsync(request.Id);
            Assert.NotNull(user);

            var claims = await userManager.GetClaimsAsync(user);
            Assert.Contains(claims, c => c.Type == ClaimTypes.GivenName && c.Value == request.FirstName);
            Assert.Contains(claims, c => c.Type == ClaimTypes.Surname && c.Value == request.LastName);
        }

        [Fact]
        public async void Can_Change_Password()
        {
            var serviceProvider = ServiceProviderHelper.CreateInstance<EditAdminUserFeatureTests>();
            var userManager = serviceProvider.Get<UserManager<AppUser>>();

            var user = new AppUser
            {
                UserName = "TestUser",
                Email = "testuser@test.com"
            };
            var password = "Testpassword@123_";
            var newPassword = "newTestPassword@00";

            var identityResult = await userManager.CreateAsync(user, password);
            Assert.True(identityResult.Succeeded);
            Assert.NotEmpty(user.Id);

            var request = new EditAdminUserFeature.Request
            {
                Id = user.Id,
                FirstName = "First",
                LastName = "Last",
                CurrentPassword = password,
                NewPassword = newPassword,
                NewPasswordConfirm = newPassword
            };

            var handler = GetHandler(serviceProvider);
            await handler.Handle(request, new CancellationToken());

            // To make sure that we changed the password, try to change it again.
            // If it succeeds, it means that previous operation was also successful.
            request.CurrentPassword = request.NewPassword;
            request.NewPassword = "ChangePassAgain@123";
            request.NewPasswordConfirm = "ChangePassAgain@123";

            await handler.Handle(request, new CancellationToken());
        }

        [Fact]
        public async void Change_Password_Throws_If_Current_Password_Not_Provided()
        {
            var serviceProvider = ServiceProviderHelper.CreateInstance<EditAdminUserFeature>();
            var userManager = serviceProvider.Get<UserManager<AppUser>>();

            var user = new AppUser
            {
                UserName = "TestUser",
                Email = "testuser@test.com"
            };
            var password = "Testpassword@123_";
            var newPassword = "newTestPassword@00";

            var identityResult = await userManager.CreateAsync(user, password);
            Assert.True(identityResult.Succeeded);
            Assert.NotEmpty(user.Id);

            var request = new EditAdminUserFeature.Request
            {
                Id = user.Id,
                FirstName = "First",
                LastName = "Last",
                NewPassword = newPassword,
                NewPasswordConfirm = newPassword
            };

            var handler = GetHandler(serviceProvider);
            await Assert.ThrowsAsync<ArgumentException>(async () => await handler.Handle(request, new CancellationToken()));
        }

        private EditAdminUserFeature.Handler GetHandler(ServiceProviderHelper serviceProvider)
        {
            var userService = serviceProvider.Get<IUserService>();

            return new EditAdminUserFeature.Handler(userService);
        }
    }
}