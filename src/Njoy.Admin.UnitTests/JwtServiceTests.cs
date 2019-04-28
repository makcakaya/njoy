using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Nensure;
using Njoy.Data;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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

        private static readonly AppUser User = new AppUser
        {
            Id = Guid.NewGuid().ToString(),
            UserName = Username,
            Email = Email
        };

        private static readonly IList<Claim> Claims = new List<Claim>()
        {
            new Claim(ClaimTypes.GivenName, "givenname"),
            new Claim(ClaimTypes.Surname, "surname"),
        };

        private static readonly IList<string> Roles = new List<string>
        {
            AppRole.AdminStandard
        };

        [Fact]
        public async void Can_Generate_Token()
        {
            var userManager = GetMockUserManager();
            userManager.Setup(u => u.FindByNameAsync(Username)).Returns(() => Task.FromResult(User));
            userManager.Setup(u => u.CheckPasswordAsync(It.IsAny<AppUser>(), It.IsAny<string>())).Returns(() => Task.FromResult(true));
            userManager.Setup(u => u.GetClaimsAsync(User)).Returns(() => Task.FromResult(Claims));
            userManager.Setup(u => u.GetRolesAsync(User)).Returns(() => Task.FromResult(Roles));
            var service = new JwtService(new JwtSettings { Secret = Secret }, userManager.Object);
            var token = await service.GenerateToken(Username, Password);

            Assert.NotEmpty(token);
        }

        [Fact]
        public async void Generated_Token_Includes_Issuer_And_Audience()
        {
            var userManager = GetMockUserManager();
            userManager.Setup(u => u.FindByNameAsync(Username)).Returns(() => Task.FromResult(User));
            userManager.Setup(u => u.CheckPasswordAsync(It.IsAny<AppUser>(), It.IsAny<string>())).Returns(() => Task.FromResult(true));
            userManager.Setup(u => u.GetClaimsAsync(User)).Returns(() => Task.FromResult(Claims));
            userManager.Setup(u => u.GetRolesAsync(User)).Returns(() => Task.FromResult(Roles));
            var service = new JwtService(
                new JwtSettings
                {
                    Secret = Secret,
                    Issuer = Issuer,
                    Audience = Audience
                },
                userManager.Object);

            var token = await service.GenerateToken(Username, Password);
            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.ReadToken(token) as JwtSecurityToken;

            Assert.Equal(Issuer, securityToken.Issuer);
            Assert.Equal(Audience, securityToken.Audiences.First());
        }

        [Fact]
        public async void Throws_If_Credentials_Are_Not_Correct()
        {
            var userManager = new Mock<UserManager<AppUser>>(
                    new Mock<IUserStore<AppUser>>().Object,
                    new Mock<IOptions<IdentityOptions>>().Object,
                    new Mock<IPasswordHasher<AppUser>>().Object,
                    new IUserValidator<AppUser>[0],
                    new IPasswordValidator<AppUser>[0],
                    new Mock<ILookupNormalizer>().Object,
                    new Mock<IdentityErrorDescriber>().Object,
                    new Mock<IServiceProvider>().Object,
                    new Mock<ILogger<UserManager<AppUser>>>().Object);

            userManager.Setup(u => u.CheckPasswordAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .Returns(() => Task.FromResult(false));

            var jwtService = new JwtService(new JwtSettings { Secret = Secret }, userManager.Object);
            await Assert.ThrowsAsync<AssertionException>(async () => await jwtService.GenerateToken("invalidusername", "Inv@lidPassword!123"));
        }

        private static Mock<UserManager<AppUser>> GetMockUserManager()
        {
            var userManager = new Mock<UserManager<AppUser>>(
                    new Mock<IUserStore<AppUser>>().Object,
                    new Mock<IOptions<IdentityOptions>>().Object,
                    new Mock<IPasswordHasher<AppUser>>().Object,
                    new IUserValidator<AppUser>[0],
                    new IPasswordValidator<AppUser>[0],
                    new Mock<ILookupNormalizer>().Object,
                    new Mock<IdentityErrorDescriber>().Object,
                    new Mock<IServiceProvider>().Object,
                    new Mock<ILogger<UserManager<AppUser>>>().Object);
            return userManager;
        }
    }
}