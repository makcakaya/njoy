using Xunit;

namespace Njoy.Admin.UnitTests
{
    public sealed class JwtServiceTests
    {
        private static readonly string Username = "testusername";
        private static readonly string Password = "TestP@ssword!123";
        private static readonly string Secret = "{0C0AAFF9-62A8-496D-835F-7998ED41D8B1}";

        [Fact]
        public void Can_Generate_Token()
        {
            var service = new JwtService();
            var token = service.GenerateToken(Username, Password, Secret);

            Assert.NotEmpty(token);
        }
    }
}