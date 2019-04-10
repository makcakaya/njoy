using Microsoft.AspNetCore.Identity;
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
            return new UserManager<AppUser>(
                new CustomUserStore(),
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null
                );
        }

        private sealed class CustomUserStore : IUserClaimStore<AppUser>, IUserRoleStore<AppUser>
        {
            private static readonly AppUser User = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = Username,
                Email = Email
            };

            private readonly IList<Claim> Claims = new List<Claim>()
            {
                new Claim(ClaimTypes.GivenName, "givenname"),
                new Claim(ClaimTypes.Surname, "surname"),
            };

            private readonly IList<string> Roles = new List<string>
            {
                AppRole.AdminStandart
            };

            public Task<AppUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
            {
                return Task.FromResult(User);
            }

            public Task<IList<Claim>> GetClaimsAsync(AppUser user, CancellationToken cancellationToken)
            {
                return Task.FromResult(Claims);
            }

            public Task<IList<string>> GetRolesAsync(AppUser user, CancellationToken cancellationToken)
            {
                return Task.FromResult(Roles);
            }

            #region NotImplemented

            public Task<string> GetNormalizedUserNameAsync(AppUser user, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }

            public Task<string> GetUserIdAsync(AppUser user, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }

            public Task<string> GetUserNameAsync(AppUser user, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }

            public Task<IList<AppUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }

            public Task<IList<AppUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }

            public Task<bool> IsInRoleAsync(AppUser user, string roleName, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }

            public Task RemoveClaimsAsync(AppUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }

            public Task RemoveFromRoleAsync(AppUser user, string roleName, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }

            public Task ReplaceClaimAsync(AppUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }

            public Task SetNormalizedUserNameAsync(AppUser user, string normalizedName, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }

            public Task SetUserNameAsync(AppUser user, string userName, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }

            public Task<IdentityResult> UpdateAsync(AppUser user, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }

            public Task AddClaimsAsync(AppUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }

            public Task AddToRoleAsync(AppUser user, string roleName, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }

            public Task<IdentityResult> CreateAsync(AppUser user, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }

            public Task<IdentityResult> DeleteAsync(AppUser user, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }

            public Task<AppUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }

            public void Dispose()
            {
                throw new NotImplementedException();
            }

            #endregion NotImplemented
        }
    }
}