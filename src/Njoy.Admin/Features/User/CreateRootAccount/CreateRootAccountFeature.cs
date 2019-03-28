using MediatR;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Njoy.Admin.Features
{
    public sealed class CreateRootAccountFeature
    {
        public sealed class Handler : AsyncRequestHandler<Request>
        {
            private readonly UserManager<AdminUser> _userManager;

            public Handler(UserManager<AdminUser> userManager)
            {
                _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            }

            protected async override Task Handle(Request request, CancellationToken cancellationToken)
            {
                if (request is null) { throw new ArgumentNullException(nameof(request)); }

                var rootUserExists = (await _userManager.GetUsersInRoleAsync(AdminRole.Root)).Count > 0;
                if (rootUserExists)
                {
                    return;
                }

                var user = new AdminUser
                {
                    UserName = request.Username
                };

                var createResult = await _userManager.CreateAsync(user, request.Password);
                if (!createResult.Succeeded)
                {
                    throw new Exception($"{nameof(Handler)} failed with following errors: {JsonConvert.SerializeObject(createResult.Errors)}");
                }

                var roleResult = await _userManager.AddToRoleAsync(user, AdminRole.Root);
                if (!roleResult.Succeeded)
                {
                    throw new Exception($"{nameof(Handler)} failed with following errors: {JsonConvert.SerializeObject(roleResult.Errors)}");
                }
            }
        }

        public sealed class Request : IRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}