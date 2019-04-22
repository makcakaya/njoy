using MediatR;
using Nensure;
using Njoy.Admin.Features;
using Njoy.Services;
using System;
using System.Threading;

namespace Njoy.Admin
{
    public static class CustomStartupTasksExtensions
    {
        public static void Run(IMediator mediator)
        {
            Ensure.NotNull(mediator);
            RunRequest(mediator, new CreateDefaultRolesFeature.Request());
            RunRequest(mediator, new CreateAdminRootUserFeature.Request());
        }

        private static void RunRequest(IMediator mediator, IRequest request)
        {
            Ensure.NotNull(mediator).NotNull(request);
            Exception exception = null;
            var blocker = new ManualResetEvent(false);
            mediator.Send(request)
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