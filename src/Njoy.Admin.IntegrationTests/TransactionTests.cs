using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Njoy.Admin.IntegrationTests
{
    public sealed class TransactionTests
    {
        #region Init

        public TransactionTests()
        {
            CleanUsersTable();
        }

        private void CleanUsersTable()
        {
            var context = ServiceProviderHelper.CreateInstance<TransactionTests>(useSqlServer: true).Get<AdminContext>();
            context.Users.RemoveRange(context.Users.ToArray());
            context.SaveChanges();
        }

        #endregion Init

        [Fact]
        public async void Transaction_Rollback_Undoes_Changes()
        {
            var serviceProvider = ServiceProviderHelper.CreateInstance<TransactionTests>(useSqlServer: true);

            const string username = "testuser123";
            await CreateUser(serviceProvider, username, true);

            var userManager = serviceProvider.Get<UserManager<AdminUser>>();

            Assert.Empty(userManager.Users);
        }

        [Fact]
        public async void Transaction_Commit_Applies_Changes()
        {
            var serviceProvider = ServiceProviderHelper.CreateInstance<TransactionTests>(useSqlServer: true);

            const string username = "testuser1234";
            await CreateUser(serviceProvider, username, false);

            var userManager = serviceProvider.Get<UserManager<AdminUser>>();
            var user = userManager.Users.FirstOrDefault(u => u.UserName == username);

            Assert.NotNull(user);
        }

        private async Task CreateUser(ServiceProviderHelper serviceProvider, string username, bool shouldFail)
        {
            var user = new AdminUser { UserName = username };
            var userManager = serviceProvider.Get<UserManager<AdminUser>>();
            var context = serviceProvider.Get<AdminContext>();

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