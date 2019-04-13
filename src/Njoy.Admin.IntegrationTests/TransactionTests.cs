using Microsoft.AspNetCore.Identity;
using Njoy.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Njoy.Admin.IntegrationTests
{
    public sealed class TransactionTests
    {
        #region Init

        // WARNING: The same instance of ServiceProviderHelper should be used to avoid running startup tasks everytime (creating users, roles etc)
        private ServiceProviderHelper ServiceProvider { get; } = ServiceProviderHelper.CreateInstance<TransactionTests>(useSqlServer: true);

        public TransactionTests()
        {
            CleanUsersTable();
        }

        private void CleanUsersTable()
        {
            var context = ServiceProvider.Get<NjoyContext>();
            context.Users.RemoveRange(context.Users.ToArray());
            context.SaveChanges();
        }

        #endregion Init

        [Fact]
        public async void Transaction_Rollback_Undoes_Changes()
        {
            const string username = "testuser123";
            await CreateUser(ServiceProvider, username, true);

            var userManager = ServiceProvider.Get<UserManager<AppUser>>();

            Assert.Empty(userManager.Users);
        }

        [Fact]
        public async void Transaction_Commit_Applies_Changes()
        {
            const string username = "testuser1234";
            await CreateUser(ServiceProvider, username, false);

            var userManager = ServiceProvider.Get<UserManager<AppUser>>();
            var user = userManager.Users.FirstOrDefault(u => u.UserName == username);

            Assert.NotNull(user);
        }

        private async Task CreateUser(ServiceProviderHelper serviceProvider, string username, bool shouldFail)
        {
            var user = new AppUser { UserName = username };
            var userManager = serviceProvider.Get<UserManager<AppUser>>();
            var context = serviceProvider.Get<NjoyContext>();

            using (var transaction = context.Database.BeginTransaction())
            {
                var result = await userManager.CreateAsync(user, "TestPassw@rd!123");
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException("Identity operation did not succeed.");
                }

                if (shouldFail)
                {
                    transaction.Rollback();
                }
                else
                {
                    context.SaveChanges();
                    transaction.Commit();
                }
            }

            return;
        }
    }
}