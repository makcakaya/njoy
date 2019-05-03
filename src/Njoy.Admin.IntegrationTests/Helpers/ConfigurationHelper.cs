using Microsoft.Extensions.Configuration;

namespace Njoy.Admin.IntegrationTests
{
    public static class ConfigurationHelper
    {
        public static readonly string FileName = "appsettings.json";

        public static IConfiguration Get()
        {
            return new ConfigurationBuilder()
                .AddJsonFile(FileName)
                .Build();
        }
    }
}