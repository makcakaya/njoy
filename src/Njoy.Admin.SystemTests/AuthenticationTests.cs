using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Njoy.Admin.Features;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using Xunit;

namespace Njoy.Admin.SystemTests
{
    public sealed class AuthenticationTests
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public AuthenticationTests()
        {
            var projectDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            _server = new TestServer(new WebHostBuilder()
                .UseEnvironment("Development")
                .UseContentRoot(projectDir)
                .UseConfiguration(new ConfigurationBuilder()
                    .SetBasePath(projectDir)
                    .AddJsonFile("appsettings.json")
                    .Build())
                .UseStartup<Startup>());
            _client = _server.CreateClient();
        }

        [Fact]
        public async void No_Attribute_Actions_Return_Unauthorized()
        {
            const string password = "TestP@ssword!234";
            var request = new CreateAdminUserFeature.Request
            {
                Username = "user123",
                Password = password,
                PasswordConfirm = password
            };

            var response = await _client.PostAsync("api/adminuser/create",
                new StringContent(JsonConvert.SerializeObject(request)));
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async void AllowAnonymous_Attribute_Actions_Does_Not_Return_Unauthorized()
        {
            const string password = "TestP@ssword!234";
            var request = new LoginAdminUserFeature.Request
            {
                Username = "user123",
                Password = password
            };

            var response = await _client.PostAsync("api/adminuser/login",
                new StringContent(JsonConvert.SerializeObject(request)));
            Assert.NotEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}