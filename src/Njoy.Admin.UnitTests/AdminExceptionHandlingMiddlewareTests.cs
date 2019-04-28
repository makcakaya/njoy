using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Nensure;
using Newtonsoft.Json;
using Njoy.Services;
using System.IO;
using System.Net;
using Xunit;

namespace Njoy.Admin.UnitTests
{
    public sealed class AdminExceptionHandlingMiddlewareTests
    {
        [Fact]
        public async void Handles_Registered_Exception_Types()
        {
            var statusCode = (int)HttpStatusCode.BadRequest;
            var middleware = new AdminExceptionHandlingMiddleware();
            var responseBodyStream = new MemoryStream();
            var exceptionMessage = "Test assertion failed.";
            var context = new DefaultHttpContext();
            context.Features.Get<IHttpResponseFeature>().Body = responseBodyStream;

            await middleware.InvokeAsync(context,
               new RequestDelegate(c => throw new AssertionException(exceptionMessage)));

            responseBodyStream.Position = 0;
            var responseBody = await new StreamReader(responseBodyStream).ReadToEndAsync();
            var exceptionContent = JsonConvert.DeserializeObject<ExceptionContent>(responseBody);

            Assert.Equal(statusCode, context.Response.StatusCode);
            Assert.NotNull(exceptionContent);
            Assert.Equal(exceptionMessage, exceptionContent.Exception.Message);
        }
    }
}