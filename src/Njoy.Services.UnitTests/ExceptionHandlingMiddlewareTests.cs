using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using Xunit;

namespace Njoy.Services.UnitTests
{
    public sealed class ExceptionHandlingMiddlewareTests
    {
        [Fact]
        public async void Handles_Registered_Exception_Types()
        {
            var responders = new ExceptionResponderCollection();
            var statusCode = (int)HttpStatusCode.BadRequest;
            var exceptionMessage = "Cannot divide by zero";
            responders.Add<DivideByZeroException>(new ExceptionResponder((ctx, ex) => statusCode));
            ExceptionHandlingMiddleware.Configure(responders);

            var middleware = new ExceptionHandlingMiddleware();
            var responseBodyStream = new MemoryStream();
            var context = new DefaultHttpContext();
            context.Features.Get<IHttpResponseFeature>().Body = responseBodyStream;

            await middleware.InvokeAsync(context,
               new RequestDelegate(c => throw new DivideByZeroException(exceptionMessage)));

            responseBodyStream.Position = 0;
            var responseBody = await new StreamReader(responseBodyStream).ReadToEndAsync();
            var thrownException = JsonConvert.DeserializeObject<DivideByZeroException>(responseBody);

            Assert.Equal(statusCode, context.Response.StatusCode);
            Assert.NotNull(thrownException);
            Assert.Equal(exceptionMessage, thrownException.Message);
        }

        [Fact]
        public async void Bypasses_If_Not_Registered_Exception_Is_Thrown()
        {
            var responders = new ExceptionResponderCollection();
            var statusCode = (int)HttpStatusCode.BadRequest;
            responders.Add<DivideByZeroException>(new ExceptionResponder((ctx, ex) => statusCode));
            ExceptionHandlingMiddleware.Configure(responders);

            var middleware = new ExceptionHandlingMiddleware();
            var responseBodyStream = new MemoryStream();
            var context = new DefaultHttpContext();
            context.Features.Get<IHttpResponseFeature>().Body = responseBodyStream;

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await middleware.InvokeAsync(context,
                   new RequestDelegate(c => throw new InvalidOperationException("Invalid operation.")));
            });

            responseBodyStream.Position = 0;
            var responseBody = await new StreamReader(responseBodyStream).ReadToEndAsync();
            var thrownException = JsonConvert.DeserializeObject<DivideByZeroException>(responseBody);

            Assert.Equal((int)HttpStatusCode.OK, context.Response.StatusCode);
            Assert.Null(thrownException);
        }

        [Fact]
        public async void Bypasses_If_Not_Configured()
        {
            var middleware = new ExceptionHandlingMiddleware();
            var responseBodyStream = new MemoryStream();
            var context = new DefaultHttpContext();
            context.Features.Get<IHttpResponseFeature>().Body = responseBodyStream;

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await middleware.InvokeAsync(context,
                   new RequestDelegate(c => throw new InvalidOperationException("Invalid operation.")));
            });

            responseBodyStream.Position = 0;
            var responseBody = await new StreamReader(responseBodyStream).ReadToEndAsync();
            var thrownException = JsonConvert.DeserializeObject<DivideByZeroException>(responseBody);

            Assert.Equal((int)HttpStatusCode.OK, context.Response.StatusCode);
            Assert.Null(thrownException);
        }
    }
}