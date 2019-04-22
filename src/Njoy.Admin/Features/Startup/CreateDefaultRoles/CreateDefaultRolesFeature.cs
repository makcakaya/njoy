using MediatR;
using Microsoft.AspNetCore.Identity;
using Nensure;
using Njoy.Data;
using Njoy.Services;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Njoy.Admin
{
    public sealed class CreateDefaultRolesFeature
    {
        public sealed class Handler : AsyncRequestHandler<Request>
        {
            private readonly NjoyContext _context;
            private readonly RoleManager<AppRole> _roleManager;

            public Handler(NjoyContext context, RoleManager<AppRole> roleManager)
            {
                Ensure.NotNull(context).NotNull(roleManager);
                _context = context;
                _roleManager = roleManager;
            }

            protected override async Task Handle(Request request, CancellationToken cancellationToken)
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    var roles = new List<string> { AppRole.AdminRoot, AppRole.AdminStandard, AppRole.Merchant };
                    foreach (var role in roles)
                    {
                        if (!await _roleManager.RoleExistsAsync(role))
                        {
                            IdentityAssert.ThrowIfFailed(await _roleManager.CreateAsync(new AppRole { Name = role }), $"{nameof(_roleManager.CreateAsync)}");
                        }
                    }

                    transaction.Commit();
                }
            }
        }

        public sealed class Request : IRequest
        {
        }
    }
}