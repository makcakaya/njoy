using Microsoft.AspNetCore.Http;
using Nensure;
using Newtonsoft.Json;
using System;

namespace Njoy.Services
{
    public sealed class ExceptionResponder : IExceptionResponder
    {
        private readonly Func<HttpContext, Exception, int> _statusCodeGenerator;

        public ExceptionResponder(Func<HttpContext, Exception, int> statusCodeGenerator)
        {
            Ensure.NotNull(statusCodeGenerator);
            _statusCodeGenerator = statusCodeGenerator;
        }

        public async void Respond(HttpContext context, Exception exception)
        {
            Ensure.NotNull(context, exception);
            var statusCode = _statusCodeGenerator(context, exception);

            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsync(JsonConvert.SerializeObject(exception));
        }
    }
}