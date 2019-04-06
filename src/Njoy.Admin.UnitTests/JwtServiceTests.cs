using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Xunit;

namespace Njoy.Admin.UnitTests
{
    public sealed class JwtServiceTests
    {
        private static readonly string Username = "testusername";
        private static readonly string Password = "TestP@ssword!123";
        private static readonly string Secret = "{0C0AAFF9-62A8-496D-835F-7998ED41D8B1}";
        private static readonly string Issuer = "TestIssuer";
        private static readonly string Audience = "TestAudience";

        [Fact]
        public void Can_Generate_Token()
        {
            var service = new JwtService(new JwtSettings { Secret = Secret });
            var token = service.GenerateToken(Username, Password);

            Assert.NotEmpty(token);
        }

        [Fact]
        public void Generated_Token_Includes_Issuer_And_Audience()
        {
            var service = new JwtService(new JwtSettings { Secret = Secret, Issuer = Issuer, Audience = Audience });
            var token = service.GenerateToken(Username, Password);
            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.ReadToken(token) as JwtSecurityToken;

            Assert.Equal(Issuer, securityToken.Issuer);
            Assert.Equal(Audience, securityToken.Audiences.First());
        }
    }
}