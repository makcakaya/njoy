using MediatR;
using Microsoft.Extensions.Configuration;
using Njoy.Admin.Features;
using Njoy.Services;
using System;
using System.Threading;

namespace Njoy.Admin
{
    public static class CustomStartupTasksExtensions
    {
        public static void CreateRootAccount(this IConfiguration config, IMediator mediator)
        {
            var blocker = new ManualResetEvent(false);

            Exception exception = null;
            mediator.Send(new CreateAdminRootUserFeature.Request())
                .ContinueWith((t) =>
                {
                    if (t.IsFaulted)
                    {
                        exception = t.Exception;
                    }
                    blocker.Set();
                });

            blocker.WaitOne();

            if (exception != null)
            {
                throw new OperationFailedException(nameof(CreateAdminRootUserFeature), exception);
            }
        }
    }
}