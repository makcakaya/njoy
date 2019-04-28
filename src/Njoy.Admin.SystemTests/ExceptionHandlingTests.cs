using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Njoy.Admin.Features;
using Njoy.Services;
using NLog;
using System.IO;
using System.Net.Http;
using System.Reflection;
using Xunit;

namespace Njoy.Admin.SystemTests
{
    public sealed class ExceptionHandlingTests
    {
        private readonly IConfiguration _config;
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public ExceptionHandlingTests()
        {
            // Build configuration object
            _config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Development.json")
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
            _client = _server.CreateClient();
        }

        [Fact]
        public async void ExceptionContent_Is_Set_If_Exception_Thrown()
        {
            var request = new LoginAdminUserFeature.Request
            {
                Username = "12345",
                Password = "nonexistinguserpassword"
            };
            var result = await _client.PostAsJsonAsync("api/adminuser/login", request);
            Assert.False(result.IsSuccessStatusCode);
            var content = await result.Content.ReadAsStringAsync();
            Assert.NotEmpty(content);
            var exceptionContent = JsonConvert.DeserializeObject<ExceptionContent>(content);
            Assert.NotNull(exceptionContent);
            Assert.NotNull(exceptionContent.Exception);
        }

        [Fact]
        public async void Context_Is_Logged_When_Exception_Is_Thrown()
        {
            const string LogName = "testall.log";
            if (File.Exists(LogName))
            {
                File.Delete(LogName);
            }

            var request = new LoginAdminUserFeature.Request
            {
                Username = "12345",
                Password = "nonexistinguserpassword"
            };
            var result = await _client.PostAsJsonAsync("api/adminuser/login", request);
            var content = await result.Content.ReadAsStringAsync();
            var exceptionContent = JsonConvert.DeserializeObject<ExceptionContent>(content);
            LogManager.Flush();
            var log = await File.ReadAllTextAsync(LogName);
            Assert.NotEmpty(log);
        }
    }
}