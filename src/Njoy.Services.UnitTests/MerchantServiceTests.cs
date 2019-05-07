using Microsoft.EntityFrameworkCore;
using Nensure;
using Njoy.Data;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Njoy.Services
{
    public sealed class MerchantServiceTests
    {
        [Fact]
        public async void Search_Returns_Only_Merchants_With_Usernames_Containing_Search_Phrase()
        {
            const int userCount = 10;
            var context = GetContext();
            var searchKey = "user";
            var usernames = new List<string>();
            for (var i = 0; i < userCount; i++)
            {
                var halfReached = i >= userCount / 2;
                var username = !halfReached ? $"test{searchKey}name{i}" : $"testname{i}";
                if (!halfReached)
                {
                    usernames.Add(username);
                }
                var user = new AppUser
                {
                    UserName = username,
                    NormalizedUserName = username.ToUpper()
                };
                context.Users.Add(user);

                var merchant = new Merchant
                {
                    UserId = user.Id,
                };
                context.Set<Merchant>().Add(merchant);
            };
            context.SaveChanges();

            var service = new MerchantService(context);
            var result = await service.Search(searchKey);
            Assert.Equal(userCount / 2, result.Count());
            for (var i = 0; i < userCount / 2; i++)
            {
                Assert.Contains(result, m => m.User.UserName == usernames[i]);
            }
        }

        [Fact]
        public async void Get_Returns_By_Merchant_Id()
        {
            var context = GetContext();
            var user = new AppUser
            {
                UserName = "testusername"
            };
            context.Set<AppUser>().Add(user);

            var merchant = new Merchant
            {
                User = user
            };
            context.Set<Merchant>().Add(merchant);
            context.SaveChanges();

            var service = new MerchantService(context);
            var result = await service.Get(merchant.Id);

            Assert.NotNull(result);
            Assert.Equal(merchant.UserId, user.Id);
        }

        [Fact]
        public async void Get_Does_Not_Throw_If_Id_Not_Found()
        {
            var context = GetContext();
            var service = new MerchantService(context);

            var result = await service.Get(13);

            Assert.Null(result);
        }

        [Fact]
        public async void Search_Throws_If_Search_Phrase_Is_Null_Or_Empty()
        {
            var context = GetContext();
            var service = new MerchantService(context);

            await Assert.ThrowsAsync<AssertionException>(() => service.Search(string.Empty));
            await Assert.ThrowsAsync<AssertionException>(() => service.Search(null));
        }

        private NjoyContext GetContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<NjoyContext>();
            optionsBuilder.UseInMemoryDatabase(nameof(MerchantServiceTests));
            return new NjoyContext(optionsBuilder.Options);
        }
    }
}