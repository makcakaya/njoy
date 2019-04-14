using Microsoft.AspNetCore.Identity;
using Moq;
using Njoy.Data;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Njoy.Admin.UnitTests
{
    public sealed class JwtServiceTests
    {
        private static readonly string Username = "testusername";
        private static readonly string Email = "testemail@mail.com";
        private static readonly string Password = "TestP@ssword!123";
        private static readonly string Secret = "{0C0AAFF9-62A8-496D-835F-7998ED41D8B1}";
        private static readonly string Issuer = "TestIssuer";
        private static readonly string Audience = "TestAudience";

        [Fact]
        public async void Can_Generate_Token()
        {
            var userManager = GetUserManager();
            var service = new JwtService(new JwtSettings { Secret = Secret }, userManager);
            var token = await service.GenerateToken(Username, Password);

            Assert.NotEmpty(token);
        }

        [Fact]
        public async void Generated_Token_Includes_Issuer_And_Audience()
        {
            var userManager = GetUserManager();
            var service = new JwtService(
                new JwtSettings
                {
                    Secret = Secret,
                    Issuer = Issuer,
                    Audience = Audience
                },
                userManager);

            var token = await service.GenerateToken(Username, Password);
            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.ReadToken(token) as JwtSecurityToken;

            Assert.Equal(Issuer, securityToken.Issuer);
            Assert.Equal(Audience, securityToken.Audiences.First());
        }

        private static UserManager<AppUser> GetUserManager()
        {
            var user = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = Username,
                Email = Email
            };

            IList<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.GivenName, "givenname"),
                new Claim(ClaimTypes.Surname, "surname"),
            };

            IList<string> roles = new List<string>
            {
                AppRole.AdminStandard
            };

            var userStore = new Mock<ITestUserStore>();
            userStore.Setup(u => u.FindByNameAsync(user.UserName, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(user));
            userStore.Setup(u => u.GetClaimsAsync(It.IsAny<AppUser>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(claims));
            userStore.Setup(u => u.GetRolesAsync(It.IsAny<AppUser>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(roles));

            return new UserManager<AppUser>(userStore.Object, null, null, null, null, null, null, null, null);
        }

        public interface ITestUserStore : IUserClaimStore<AppUser>, IUserRoleStore<AppUser>
        {
        }
    }
}