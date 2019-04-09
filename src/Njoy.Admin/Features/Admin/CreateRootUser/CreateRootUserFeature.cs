using MediatR;
using Microsoft.AspNetCore.Identity;
using Njoy.Data;
using Njoy.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Njoy.Admin.Features
{
    public sealed class CreateRootUserFeature
    {
        public sealed class Handler : AsyncRequestHandler<Request>
        {
            private readonly NjoyContext _context;
            private readonly UserManager<AppUser> _userManager;
            private readonly RoleManager<AppRole> _roleManager;

            public Handler(NjoyContext context, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
                _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
                _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            }

            protected async override Task Handle(Request request, CancellationToken cancellationToken)
            {
                request = request ?? throw new ArgumentNullException(nameof(request));

                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    var rootUserExists = (await _userManager.GetUsersInRoleAsync(AppRole.AdminRoot)).Count > 0;
                    if (rootUserExists) { return; }

                    var user = new AppUser
                    {
                        UserName = request.Username
                    };

                    IdentityAssert.ThrowIfFailed(await _userManager.CreateAsync(user, request.Password), "Create root user");

                    if (!await _roleManager.RoleExistsAsync(AppRole.AdminRoot))
                    {
                        IdentityAssert.ThrowIfFailed(await _roleManager.CreateAsync(new AppRole { Name = AppRole.AdminRoot }), "Create root role");
                    }

                    IdentityAssert.ThrowIfFailed(await _userManager.AddToRoleAsync(user, AppRole.AdminRoot), "Add user to root role");

                    transaction.Commit();
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