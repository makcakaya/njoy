using Microsoft.AspNetCore.Http;
using Nensure;
using System;
using System.Threading.Tasks;

namespace Njoy.Services
{
    public sealed class ExceptionHandlingMiddleware : IMiddleware
    {
        private static ExceptionResponderCollection Responders { get; set; }

        public static void Configure(ExceptionResponderCollection responders)
        {
            Ensure.NotNull(responders);
            Responders = responders;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                if (Responders != null)
                {
                    var responder = Responders[ex.GetType()];
                    if (responder != null)
                    {
                        responder.Respond(context, ex);
                        return;
                    }
                }

                throw;
            }
        }

        public Task InvokeAsync(DefaultHttpContext defaultHttpContext)
        {
            throw new NotImplementedException();
        }
    }
}