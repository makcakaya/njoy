using MediatR;
using Microsoft.AspNetCore.Identity;
using Njoy.Admin.Features;
using Njoy.Data;
using System.Linq;
using System.Threading;
using Xunit;

namespace Njoy.Admin.IntegrationTests
{
    public sealed class CreateRootUserFeatureTests
    {
        [Fact]
        public async void Can_Create_Root_User()
        {
            var serviceProvider = ServiceProviderHelper.CreateInstance<CreateRootUserFeature>();
            var userManager = serviceProvider.Get<UserManager<AppUser>>();
            var handler = GetHandler(serviceProvider) as IRequestHandler<CreateRootUserFeature.Request, Unit>;

            var request = new CreateRootUserFeature.Request
            {
                Username = "rootuser1",
                Password = "testP@ssword!1"
            };

            await handler.Handle(request, new CancellationToken());

            var user = userManager.Users.FirstOrDefault(u => u.UserName == request.Username);
            Assert.NotNull(user);
        }

        [Fact]
        public async void Created_Root_User_Has_Root_Role()
        {
            var serviceProvider = ServiceProviderHelper.CreateInstance<CreateRootUserFeature>();
            var userManager = serviceProvider.Get<UserManager<AppUser>>();
            var handler = GetHandler(serviceProvider) as IRequestHandler<CreateRootUserFeature.Request, Unit>;

            var request = new CreateRootUserFeature.Request
            {
                Username = "rootuser1",
                Password = "testP@ssword!1"
            };

            await handler.Handle(request, new CancellationToken());

            var user = userManager.Users.FirstOrDefault(u => u.UserName == request.Username);
            var roles = await userManager.GetRolesAsync(user);

            Assert.Contains(AppRole.AdminRoot, roles);
        }

        [Fact]
        public async void Does_Not_Create_Duplicate_Root_Users()
        {
            var serviceProvider = ServiceProviderHelper.CreateInstance<CreateRootUserFeature>();
            var userManager = serviceProvider.Get<UserManager<AppUser>>();
            var handler = GetHandler(serviceProvider) as IRequestHandler<CreateRootUserFeature.Request, Unit>;

            var request = new CreateRootUserFeature.Request
            {
                Username = "rootuser1",
                Password = "testP@ssword!1"
            };

            await handler.Handle(request, new CancellationToken());

            request.Username = "rootuser2";
            await handler.Handle(request, new CancellationToken());

            var rootUsers = await userManager.GetUsersInRoleAsync(AppRole.AdminRoot);

            Assert.Single(rootUsers);
        }

        private CreateRootUserFeature.Handler GetHandler(ServiceProviderHelper serviceProvider)
        {
            var context = serviceProvider.Get<NjoyContext>();
            var userManager = serviceProvider.Get<UserManager<AppUser>>();
            var roleManager = serviceProvider.Get<RoleManager<AppRole>>();

            return new CreateRootUserFeature.Handler(context, userManager, roleManager);
        }
    }
}