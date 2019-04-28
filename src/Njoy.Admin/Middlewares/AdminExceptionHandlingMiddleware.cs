using FluentValidation;
using Microsoft.AspNetCore.Http;
using Nensure;
using Newtonsoft.Json;
using Njoy.Services;
using System;
using System.Threading.Tasks;

namespace Njoy.Admin
{
    public sealed class AdminExceptionHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (AssertionException ex)
            {
                await SetResponse(context.Response, StatusCodes.Status400BadRequest, ex);
            }
            catch (ValidationException ex)
            {
                await SetResponse(context.Response, StatusCodes.Status400BadRequest, ex);
            }
            catch (Exception ex)
            {
                await SetResponse(context.Response, StatusCodes.Status500InternalServerError, ex);
            }
        }

        private async Task SetResponse(HttpResponse response, int statusCode, Exception exception)
        {
            Ensure.NotNull(response, exception);
            response.StatusCode = statusCode;
            await response.WriteAsync(JsonConvert.SerializeObject(new ExceptionContent(exception)));
        }
    }
}