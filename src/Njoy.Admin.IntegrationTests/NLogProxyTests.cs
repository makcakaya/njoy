using Microsoft.Extensions.Logging;
using System.IO;
using Xunit;

namespace Njoy.Admin.IntegrationTests
{
    public sealed class NLogProxyTests
    {
        [Fact]
        public void Can_Write_To_File()
        {
            const string FileName = "test.log";

            var logger = new NLogProxy<NLogProxyTests>();
            logger.Log(LogLevel.Critical, "Test test test");

            var content = File.ReadAllText(FileName);

            Assert.NotEmpty(content);
        }
    }
}