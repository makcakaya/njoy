using Microsoft.AspNetCore.Mvc.Filters;
using Nensure;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Njoy.Admin
{
    public sealed class AdminExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;

        public AdminExceptionFilterAttribute(ILogger logger)
        {
            Ensure.NotNull(logger);
            _logger = logger;
        }

        public override Task OnExceptionAsync(ExceptionContext context)
        {
            // Just log the context and let the global exception handler do the rest.
            _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, context.Exception, JsonConvert.SerializeObject(context));
            return Task.CompletedTask;
        }
    }
}