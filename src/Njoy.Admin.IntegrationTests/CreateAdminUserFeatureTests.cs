using Microsoft.AspNetCore.Identity;
using Njoy.Admin.Features;
using Njoy.Data;
using Njoy.Services;
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
            var userManager = serviceProvider.Get<UserManager<AppUser>>();
            var handler = GetHandler(serviceProvider);

            var request = new CreateAdminUserFeature.Request
            {
                Username = "adminuser1",
                Password = "testP@ssword!1",
                PasswordConfirm = "testP@ssword!1",
                FirstName = "AdminName",
                LastName = "AdminSurname",
                Email = "admin@test.com",
                Role = AppRole.AdminStandart
            };

            var createdUser = await handler.Handle(request, new CancellationToken());
            var user = userManager.Users.FirstOrDefault(u => u.UserName == request.Username);

            Assert.NotNull(user);
            Assert.Equal(user.Id, createdUser.Id);

            var roles = await userManager.GetRolesAsync(user);
            Assert.Contains(request.Role, roles);
        }

        [Fact]
        public async void Do_Not_Create_With_Existing_Username()
        {
            var request = new CreateAdminUserFeature.Request
            {
                Username = "adminuser1",
                Password = "testP@ssword!1",
                PasswordConfirm = "testP@ssword!1",
                FirstName = "AdminName",
                LastName = "AdminSurname",
                Email = "admin@test.com",
                Role = AppRole.AdminStandart
            };

            var handler = GetHandler(ServiceProviderHelper.CreateInstance<CreateAdminUserFeatureTests>());

            await handler.Handle(request, new CancellationToken());

            await Assert.ThrowsAsync<OperationFailedException>(async () => await handler.Handle(request, new CancellationToken()));
        }

        private CreateAdminUserFeature.Handler GetHandler(ServiceProviderHelper serviceProvider)
        {
            var context = serviceProvider.Get<NjoyContext>();
            var userManager = serviceProvider.Get<UserManager<AppUser>>();
            var roleManager = serviceProvider.Get<RoleManager<AppRole>>();

            return new CreateAdminUserFeature.Handler(new UserService(context, userManager, roleManager));
        }
    }
}