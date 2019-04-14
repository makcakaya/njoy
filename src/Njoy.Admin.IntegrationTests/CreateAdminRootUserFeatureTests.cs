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
        private ServiceProviderHelper ServiceProvider { get; } = ServiceProviderHelper.CreateInstance<CreateAdminRootUserFeature>();

        public CreateAdminRootUserFeatureTests()
        {
            var context = ServiceProvider.Get<NjoyContext>();
            context.Users.RemoveRange(context.Users);
            context.SaveChanges();
        }

        [Fact]
        public async void Can_Create_Root_User()
        {
            var userManager = ServiceProvider.Get<UserManager<AppUser>>();
            var adminRootConfig = GetAdminRootConfig();
            var handler = GetHandler(ServiceProvider, adminRootConfig) as IRequestHandler<CreateAdminRootUserFeature.Request, Unit>;

            var request = new CreateAdminRootUserFeature.Request();
            await handler.Handle(request, new CancellationToken());

            var user = userManager.Users.FirstOrDefault(u => u.UserName == adminRootConfig.Username);
            Assert.NotNull(user);
        }

        [Fact]
        public async void Created_Root_User_Has_Root_Role()
        {
            var userManager = ServiceProvider.Get<UserManager<AppUser>>();
            var adminRootConfig = GetAdminRootConfig();
            var handler = GetHandler(ServiceProvider, adminRootConfig) as IRequestHandler<CreateAdminRootUserFeature.Request, Unit>;

            var request = new CreateAdminRootUserFeature.Request();
            await handler.Handle(request, new CancellationToken());

            var user = userManager.Users.FirstOrDefault(u => u.UserName == adminRootConfig.Username);
            var roles = await userManager.GetRolesAsync(user);

            Assert.Contains(AppRole.AdminRoot, roles);
        }

        [Fact]
        public async void Does_Not_Create_Multiple_Root_Users_If_Not_Allowed()
        {
            var userManager = ServiceProvider.Get<UserManager<AppUser>>();
            var adminRootConfig = GetAdminRootConfig();
            var handler = GetHandler(ServiceProvider, adminRootConfig) as IRequestHandler<CreateAdminRootUserFeature.Request, Unit>;

            var request = new CreateAdminRootUserFeature.Request();
            await handler.Handle(request, new CancellationToken());

            adminRootConfig.Username = "NewTestAdminRoot";
            handler = GetHandler(ServiceProvider, adminRootConfig) as IRequestHandler<CreateAdminRootUserFeature.Request, Unit>;
            await handler.Handle(request, new CancellationToken());

            var rootUsers = await userManager.GetUsersInRoleAsync(AppRole.AdminRoot);
            Assert.Single(rootUsers);
        }

        [Fact]
        public async void Creates_Multiple_Root_Users_If_Allowed()
        {
            var userManager = ServiceProvider.Get<UserManager<AppUser>>();
            var adminRootConfig = GetAdminRootConfig();
            adminRootConfig.AllowMultipleRootUsers = true;
            var handler = GetHandler(ServiceProvider, adminRootConfig) as IRequestHandler<CreateAdminRootUserFeature.Request, Unit>;

            var request = new CreateAdminRootUserFeature.Request();
            await handler.Handle(request, new CancellationToken());

            adminRootConfig.Username = "NewTestAdminRoot0";
            handler = GetHandler(ServiceProvider, adminRootConfig) as IRequestHandler<CreateAdminRootUserFeature.Request, Unit>;
            await handler.Handle(request, new CancellationToken());

            adminRootConfig.Username = "NewTestAdminRoot1";
            handler = GetHandler(ServiceProvider, adminRootConfig) as IRequestHandler<CreateAdminRootUserFeature.Request, Unit>;
            await handler.Handle(request, new CancellationToken());

            var rootUsers = await userManager.GetUsersInRoleAsync(AppRole.AdminRoot);
            Assert.True(rootUsers.Count > 1);
        }

        private CreateAdminRootUserFeature.Handler GetHandler(ServiceProviderHelper serviceProvider,
            AdminRootConfig adminRootConfig)
        {
            var userService = serviceProvider.Get<IUserService>();
            var userManager = serviceProvider.Get<UserManager<AppUser>>();
            return new CreateAdminRootUserFeature.Handler(userService, userManager, adminRootConfig);
        }

        private AdminRootConfig GetAdminRootConfig()
        {
            return new AdminRootConfig
            {
                Username = "Testadminroot",
                Password = "TestP@ssword!123",
                Email = "test@test.com",
                FirstName = "Admin",
                LastName = "Root",
                AllowMultipleRootUsers = false
            };
        }
    }
}