using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Njoy.Admin.Features
{
    public sealed class ListAdminUsersFeature
    {
        public sealed class Handler : IRequestHandler<Request, List<AdminUserRowModel>>
        {
            private readonly UserManager<AdminUser> _userManager;

            public Handler(UserManager<AdminUser> userManager)
            {
                _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            }

            public async Task<List<AdminUserRowModel>> Handle(Request request, CancellationToken cancellationToken)
            {
                var adminUsers = await _userManager.GetUsersInRoleAsync(AdminRole.Sales);
                if (!request.ListAllUsers)
                {
                    if (!string.IsNullOrEmpty(request.IdFilter))
                    {
                        adminUsers = adminUsers.Where(us => us.Id == request.IdFilter).ToList();
                    }
                    else if (!string.IsNullOrEmpty(request.UsernameFilter))
                    {
                        adminUsers = adminUsers.Where(us => us.UserName == request.UsernameFilter).ToList();
                    }
                }

                var listAdminUsers = new List<AdminUserRowModel>();
                foreach (var user in adminUsers)
                {
                    var claims = await _userManager.GetClaimsAsync(user);
                    listAdminUsers.Add(new AdminUserRowModel()
                    {
                        Id = user.Id,
                        Email = user.Email,
                        Username = user.UserName,
                        FirstName = claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value,
                        LastName = claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value
                    });
                }

                return listAdminUsers;
            }
        }

        public sealed class Request : IRequest<List<AdminUserRowModel>>
        {
            public string IdFilter { get; set; }
            public string UsernameFilter { get; set; }

            public bool ListAllUsers => string.IsNullOrEmpty(IdFilter) && string.IsNullOrEmpty(UsernameFilter);
        }
    } 
}
