using Microsoft.AspNetCore.Identity;
using Njoy.Admin.Features;
using System;
using System.Linq;
using System.Threading;
using Xunit;

namespace Njoy.Admin.IntegrationTests
{
    public class CreateAdminUserFeatureTests
    {
        [Fact]
        public async void Can_Create_Admin_User()
        {
            var serviceProvider = ServiceProviderHelper.CreateInstance<CreateAdminUserFeature>();
            var userManager = serviceProvider.Get<UserManager<AdminUser>>();
            var handler = GetHandler(serviceProvider);

            var request = new CreateAdminUserFeature.Request
            {
                Username = "adminuser1",
                NewPassword = "testP@ssword!1",
                NewPasswordConfirm = "testP@ssword!1",
                FirstName = "AdminName",
                LastName = "AdminSurname",
                Email = "admin@test.com"
            };

            var createdUser = await handler.Handle(request, new CancellationToken());
            var user = userManager.Users.FirstOrDefault(u => u.UserName == request.Username);

            Assert.NotNull(user);
            Assert.Equal(user.Id, createdUser.Id);

            var roles = await userManager.GetRolesAsync(user);
            Assert.Contains(AdminRole.Sales, roles);
        }

        [Fact]
        public async void Do_Not_Create_With_Existing_Username()
        {
            var request = new CreateAdminUserFeature.Request
            {
                Username = "adminuser1",
                NewPassword = "testP@ssword!1",
                NewPasswordConfirm = "testP@ssword!1"
            };

            var handler = GetHandler(ServiceProviderHelper.CreateInstance<CreateAdminUserFeatureTests>());

            await handler.Handle(request, new CancellationToken());

            await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(request, new CancellationToken()));
        }

        private CreateAdminUserFeature.Handler GetHandler(ServiceProviderHelper serviceProvider)
        {
            var context = serviceProvider.Get<AdminContext>();
            var userManager = serviceProvider.Get<UserManager<AdminUser>>();
            var roleManager = serviceProvider.Get<RoleManager<AdminRole>>();
            return new CreateAdminUserFeature.Handler(context, userManager, roleManager);
        }
    }
}