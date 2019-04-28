using Microsoft.AspNetCore.Mvc.Filters;
using Njoy.Services;
using System.Threading.Tasks;

namespace Njoy.Admin
{
    public sealed class AdminExceptionFilterAttribute : ExceptionFilterAttribute, IConfigurableFilter<ILogger>
    {
        private ILogger _logger = new NLogProxy<AdminExceptionFilterAttribute>();

        public void Configure(ILogger logger)
        {
            _logger = logger;
        }

        public override Task OnExceptionAsync(ExceptionContext context)
        {
            // Just log the context and let the global exception handler do the rest.
            _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, context.Exception, "Unhandled exception", context);
            return Task.CompletedTask;
        }
    }
}