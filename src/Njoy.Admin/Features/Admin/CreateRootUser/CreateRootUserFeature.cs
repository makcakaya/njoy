using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Njoy.Admin.Features
{
    public sealed class CreateRootUserFeature
    {
        public sealed class Handler : AsyncRequestHandler<Request>
        {
            private readonly UserManager<AdminUser> _userManager;
            private readonly RoleManager<AdminRole> _roleManager;

            public Handler(UserManager<AdminUser> userManager, RoleManager<AdminRole> roleManager)
            {
                _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
                _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            }

            protected async override Task Handle(Request request, CancellationToken cancellationToken)
            {
                request = request ?? throw new ArgumentNullException(nameof(request));

                var rootUserExists = (await _userManager.GetUsersInRoleAsync(AdminRole.Root)).Count > 0;
                if (rootUserExists) { return; }

                var user = new AdminUser
                {
                    UserName = request.Username
                };

                IdentityAssert.ThrowIfFailed(await _userManager.CreateAsync(user, request.Password), "Create root user");

                if (!await _roleManager.RoleExistsAsync(AdminRole.Root))
                {
                    IdentityAssert.ThrowIfFailed(await _roleManager.CreateAsync(new AdminRole { Name = AdminRole.Root }), "Create root role");
                }

                IdentityAssert.ThrowIfFailed(await _userManager.AddToRoleAsync(user, AdminRole.Root), "Add user to root role");
            }
        }

        public sealed class Request : IRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}