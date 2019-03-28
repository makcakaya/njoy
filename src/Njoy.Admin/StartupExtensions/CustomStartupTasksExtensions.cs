using MediatR;
using Njoy.Admin.Features;
using System.Threading;

namespace Njoy.Admin
{
    public static class CustomStartupTasksExtensions
    {
        public static void CreateRootAccount(IMediator mediator)
        {
            var blocker = new ManualResetEvent(false);
            var request = new CreateRootAccountFeature.Request
            {
                Username = "root",
                Password = "Password@123"
            };

            mediator.Send(request)
            .ContinueWith((t) =>
            {
                blocker.Set();
            });

            blocker.WaitOne();
        }
    }
}