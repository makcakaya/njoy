using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Njoy.Admin.Features;
using Njoy.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Njoy.Admin.SystemTests
{
    public sealed class AuthenticationTests
    {
        private readonly IConfiguration _config;
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public AuthenticationTests()
        {
            // Build configuration object
            _config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            // Setup & start the server
            var projectDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            _server = new TestServer(new WebHostBuilder()
                .UseEnvironment("Development")
                .UseContentRoot(projectDir)
                .UseConfiguration(new ConfigurationBuilder()
                    .SetBasePath(projectDir)
                    .AddJsonFile("appsettings.json")
                    .Build())
                .UseStartup<Startup>());

            CleanDbUsersRoles(_server.Host.Services.GetService(typeof(NjoyContext)) as NjoyContext);
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
                new StringContent(JsonConvert.SerializeObject(request),
                Encoding.UTF8, MediaTypeNames.Application.Json));
            Assert.NotEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData(AppRole.AdminStandard)]
        public async void No_Attribute_Actions_Return_Unauthorized_If_Not_In_Role(string role)
        {
            const string username = "Testuser";
            const string password = "Testp@ssword!123";
            const string email = "test@test.com";
            const string firstName = "Test";
            const string lastName = "User";

            var rootUserConfig = _config.GetSection("AppDefaults").Get<AppDefaultsConfig>().AdminRoot;
            await Login(rootUserConfig.Username, rootUserConfig.Password);

            // Register a new user as the root user, if not exists that does not have the role
            var createUserRequest = new CreateAdminUserFeature.Request
            {
                Username = username,
                Password = password,
                PasswordConfirm = password,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                Role = role,
            };

            var createUserResponse = await _client.PostAsJsonAsync($"{AdminUserController.Route}/create", createUserRequest);
            Assert.True(createUserResponse.IsSuccessStatusCode);

            // Login with the newly created user
            await Login(createUserRequest.Username, createUserRequest.Password);

            // Call a restricted action. The action should return Forbidden
            var response = await _client.PostAsJsonAsync($"{AdminUserController.Route}/create", "{}");
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        private async Task Login(string username, string password)
        {
            var loginRequest = new LoginAdminUserFeature.Request
            {
                Username = username,
                Password = password
            };

            var loginResponse = await _client.PostAsJsonAsync($"{AdminUserController.Route}/login", loginRequest);
            Assert.True(loginResponse.IsSuccessStatusCode);

            var jwtToken = await loginResponse.Content.ReadAsStringAsync();
            Assert.NotEmpty(jwtToken);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            return;
        }

        private void CleanDbUsersRoles(NjoyContext context)
        {
            var rootUserConfig = _config.GetSection("AppDefaults").Get<AppDefaultsConfig>().AdminRoot;
            context.Users.RemoveRange(context.Users.Where(u => u.UserName != rootUserConfig.Username));
            context.SaveChanges();
        }
    }
}