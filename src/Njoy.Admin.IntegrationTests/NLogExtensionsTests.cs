using SimpleInjector;
using Xunit;

namespace Njoy.Admin.IntegrationTests
{
    public sealed class NLogExtensionsTests
    {
        [Fact]
        public void RegisterNLog_Registers_ILogger()
        {
            var container = new Container();
            container.RegisterNLog();
            container.Register<LoggerConsumer>();

            container.Verify();

            var consumer = container.GetInstance<LoggerConsumer>();

            Assert.NotNull(consumer.Logger);
        }

        private sealed class LoggerConsumer
        {
            public ILogger Logger { get; }

            public LoggerConsumer(ILogger logger)
            {
                Logger = logger;
            }
        }
    }
}