using MediatR;
using Microsoft.Extensions.Configuration;
using Njoy.Admin.Features;
using System.Threading;

namespace Njoy.Admin
{
    public static class CustomStartupTasksExtensions
    {
        public static void CreateRootAccount(this IConfiguration config, IMediator mediator)
        {
            const string SectionName = "RootUser";
            const string UsernameKey = "username";
            const string PasswordKey = "password";

            var blocker = new ManualResetEvent(false);

            var section = config.GetSection(SectionName);
            var request = new CreateRootAccountFeature.Request
            {
                Username = section[UsernameKey],
                Password = section[PasswordKey]
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