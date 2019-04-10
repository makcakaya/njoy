using MediatR;
using Microsoft.AspNetCore.Identity;
using Njoy.Admin.Features;
using Njoy.Data;
using Njoy.Services;
using System.Linq;
using System.Threading;
using Xunit;

namespace Njoy.Admin.IntegrationTests
{
    public sealed class CreateAdminRootUserFeatureTests
    {
        public static readonly AdminRootConfig AdminRootConfig = new AdminRootConfig
        {
            Username = "Testadminroot",
            Password = "TestP@ssword!123",
            Email = "test@test.com",
            FirstName = "Admin",
            LastName = "Root"
        };

        [Fact]
        public async void Can_Create_Root_User()
        {
            var serviceProvider = ServiceProviderHelper.CreateInstance<CreateAdminRootUserFeature>();
            var userManager = serviceProvider.Get<UserManager<AppUser>>();
            var handler = GetHandler(serviceProvider, AdminRootConfig) as IRequestHandler<CreateAdminRootUserFeature.Request, Unit>;

            var request = new CreateAdminRootUserFeature.Request();
            await handler.Handle(request, new CancellationToken());

            var user = userManager.Users.FirstOrDefault(u => u.UserName == AdminRootConfig.Username);
            Assert.NotNull(user);
        }

        [Fact]
        public async void Created_Root_User_Has_Root_Role()
        {
            var serviceProvider = ServiceProviderHelper.CreateInstance<CreateAdminRootUserFeature>();
            var userManager = serviceProvider.Get<UserManager<AppUser>>();
            var handler = GetHandler(serviceProvider, AdminRootConfig) as IRequestHandler<CreateAdminRootUserFeature.Request, Unit>;

            var request = new CreateAdminRootUserFeature.Request();
            await handler.Handle(request, new CancellationToken());

            var user = userManager.Users.FirstOrDefault(u => u.UserName == AdminRootConfig.Username);
            var roles = await userManager.GetRolesAsync(user);

            Assert.Contains(AppRole.AdminRoot, roles);
        }

        [Fact]
        public async void Does_Not_Create_Duplicate_Root_Users()
        {
            var serviceProvider = ServiceProviderHelper.CreateInstance<CreateAdminRootUserFeature>();
            var userManager = serviceProvider.Get<UserManager<AppUser>>();
            var handler = GetHandler(serviceProvider, AdminRootConfig) as IRequestHandler<CreateAdminRootUserFeature.Request, Unit>;

            var request = new CreateAdminRootUserFeature.Request();
            await handler.Handle(request, new CancellationToken());

            AdminRootConfig.Username = "NewTestAdminRoot";
            handler = GetHandler(serviceProvider, AdminRootConfig) as IRequestHandler<CreateAdminRootUserFeature.Request, Unit>;
            await handler.Handle(request, new CancellationToken());

            var rootUsers = await userManager.GetUsersInRoleAsync(AppRole.AdminRoot);
            Assert.Single(rootUsers);
        }

        private CreateAdminRootUserFeature.Handler GetHandler(ServiceProviderHelper serviceProvider,
            AdminRootConfig adminRootConfig)
        {
            var userService = serviceProvider.Get<IUserService>();
            return new CreateAdminRootUserFeature.Handler(userService, adminRootConfig);
        }
    }
}