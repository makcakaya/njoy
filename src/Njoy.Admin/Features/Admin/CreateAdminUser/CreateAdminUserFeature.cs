using MediatR;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Njoy.Admin.Features
{
    public sealed class CreateAdminUserFeature
    {
        public sealed class Handler : IRequestHandler<Request, AdminUserRowModel>
        {
            private readonly AdminContext _context;
            private readonly UserManager<AdminUser> _userManager;
            private readonly RoleManager<AdminRole> _roleManager;

            public Handler(AdminContext context, UserManager<AdminUser> userManager, RoleManager<AdminRole> roleManager)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
                _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
                _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            }

            public async Task<AdminUserRowModel> Handle(Request request, CancellationToken cancellationToken)
            {
                if (!request.IsValid())
                {
                    throw new ArgumentException("Request is not valid.");
                }

                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    if (_userManager.Users.Any(u => u.UserName == request.Username))
                    {
                        throw new Exception($"{nameof(AdminUser)} with {request.Username} already exist.");
                    }

                    var user = new AdminUser
                    {
                        UserName = request.Username,
                        Email = request.Email,
                    };

                    IdentityAssert.ThrowIfFailed(await _userManager.CreateAsync(user, request.NewPassword), "Create user");

                    // Add claims; FirstName, LastName
                    AddClaim(user, ClaimTypes.GivenName, request.FirstName);
                    AddClaim(user, ClaimTypes.Surname, request.LastName);

                    if (!await _roleManager.RoleExistsAsync(AdminRole.Sales))
                    {
                        IdentityAssert.ThrowIfFailed(await _roleManager.CreateAsync(new AdminRole { Name = AdminRole.Sales }), "Create Sales role");
                    }

                    IdentityAssert.ThrowIfFailed(await _userManager.AddToRoleAsync(user, AdminRole.Sales), "Add user to Sales role");
                    transaction.Commit();

                    var claims = await _userManager.GetClaimsAsync(user);
                    return new AdminUserRowModel
                    {
                        Id = user.Id,
                        Username = user.UserName,
                        FirstName = claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value,
                        LastName = claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value,
                        Email = user.Email
                    };
                }
            }

            private async void AddClaim(AdminUser user, string claimType, string value)
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    var result = await _userManager.AddClaimAsync(user, new Claim(claimType, value));
                    IdentityAssert.ThrowIfFailed(result, $"Adding claim {claimType}");
                }
            }
        }

        public sealed class Request : IRequest<AdminUserRowModel>
        {
            public string Id { get; set; }

            [Required]
            public string Username { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            [EmailAddress]
            public string Email { get; set; }

            [MinLength(6)]
            public string NewPassword { get; set; }

            [MinLength(6)]
            public string NewPasswordConfirm { get; set; }

            public bool IsValid()
            {
                return NewPassword == NewPasswordConfirm && NewPassword != null;
            }
        }
    }
}