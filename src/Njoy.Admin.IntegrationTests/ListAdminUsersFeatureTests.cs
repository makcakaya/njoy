using Microsoft.AspNetCore.Identity;
using Njoy.Admin.Features;
using Njoy.Data;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Njoy.Admin.IntegrationTests
{
    public sealed class ListAdminUsersFeatureTests
    {
        [Fact]
        public async void Can_List_Created_Admin_Users()
        {
            const int userCount = 5; // number of users to create for test

            var serviceProvider = ServiceProviderHelper.CreateInstance<ListAdminUsersFeatureTests>();
            var userManager = serviceProvider.Get<UserManager<AppUser>>();
            var roleManager = serviceProvider.Get<RoleManager<AppRole>>();

            var users = await CreateBulkUsers(userCount, userManager, roleManager);

            // list inserted users
            var request = new ListAdminUsersFeature.Request();
            var handler = new ListAdminUsersFeature.Handler(userManager);
            var listUsers = await handler.Handle(request, new CancellationToken());

            // assetion
            Assert.True(listUsers.Count == users.Count);
            for (int i = 0; i < listUsers.Count; i++)
            {
                var createdUser = users[i];
                var returnedUser = listUsers[i];
                var createdClaims = await userManager.GetClaimsAsync(createdUser);
                Assert.Equal(createdUser.Id, returnedUser.Id);
                Assert.Equal(createdUser.UserName, returnedUser.Username);
                Assert.Equal(createdUser.Email, returnedUser.Email);
                Assert.Equal(createdClaims[0].Value, returnedUser.FirstName);
                Assert.Equal(createdClaims[1].Value, returnedUser.LastName);
            }
        }

        [Fact]
        public async void Can_Get_User_By_Id()
        {
            const int userCount = 2; // number of users to create for test

            var serviceProvider = ServiceProviderHelper.CreateInstance<ListAdminUsersFeatureTests>();
            var userManager = serviceProvider.Get<UserManager<AppUser>>();
            var roleManager = serviceProvider.Get<RoleManager<AppRole>>();

            var users = await CreateBulkUsers(userCount, userManager, roleManager);

            for (int i = 0; i < users.Count; i++)
            {
                var createdUser = users[i];

                // get admin user by id
                var request = new ListAdminUsersFeature.Request
                {
                    IdFilter = createdUser.Id
                };
                var handler = new ListAdminUsersFeature.Handler(userManager);
                var listUsers = await handler.Handle(request, new CancellationToken());

                // assetion
                Assert.True(listUsers.Count == 1);
                var returnedUser = listUsers[0];

                Assert.Equal(createdUser.Id, returnedUser.Id);
                Assert.Equal(createdUser.UserName, returnedUser.Username);
                Assert.Equal(createdUser.Email, returnedUser.Email);
            }
        }

        [Fact]
        public async void Can_Get_User_By_Username()
        {
            const int userCount = 2; // number of users to create for test

            var serviceProvider = ServiceProviderHelper.CreateInstance<ListAdminUsersFeatureTests>();
            var userManager = serviceProvider.Get<UserManager<AppUser>>();
            var roleManager = serviceProvider.Get<RoleManager<AppRole>>();

            var users = await CreateBulkUsers(userCount, userManager, roleManager);

            for (int i = 0; i < users.Count; i++)
            {
                var createdUser = users[i];

                // get admin user by id
                var request = new ListAdminUsersFeature.Request
                {
                    UsernameFilter = createdUser.UserName
                };
                var handler = new ListAdminUsersFeature.Handler(userManager);
                var listUsers = await handler.Handle(request, new CancellationToken());

                // assetion
                Assert.True(listUsers.Count == 1);
                var returnedUser = listUsers[0];

                Assert.Equal(createdUser.Id, returnedUser.Id);
                Assert.Equal(createdUser.UserName, returnedUser.Username);
                Assert.Equal(createdUser.Email, returnedUser.Email);
            }
        }

        private async Task<List<AppUser>> CreateBulkUsers(int userCount, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            // create some test users
            var users = new List<AppUser>();
            for (int i = 0; i < userCount; i++)
            {
                users.Add(new AppUser
                {
                    UserName = $"admin{i}",
                    Email = $"email{i}"
                });
            }

            if (!await roleManager.RoleExistsAsync(AppRole.Sales))
            {
                await roleManager.CreateAsync(new AppRole { Name = AppRole.Sales });
            }
            for (int i = 0; i < users.Count; i++)
            {
                var user = users[i];
                await userManager.CreateAsync(user, "Password_1234");
                await userManager.AddClaimAsync(user, new Claim(ClaimTypes.GivenName, $"name{i}"));
                await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Surname, $"surname{i}"));
                await userManager.AddToRoleAsync(user, AppRole.Sales);
            }

            return users;
        }
    }
}