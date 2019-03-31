using MediatR;
using Microsoft.Extensions.Configuration;
using Njoy.Admin.Features;
using System;
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
            if (section is null) { return; }

            var request = new CreateRootUserFeature.Request
            {
                Username = section[UsernameKey],
                Password = section[PasswordKey]
            };

            if (request.Username is null || request.Password is null) { return; }

            Exception exception = null;
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
                throw new Exception($"{nameof(CreateRootAccount)} failed.", exception);
            }
        }
    }
}