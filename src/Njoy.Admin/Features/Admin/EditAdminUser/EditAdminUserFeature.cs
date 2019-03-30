﻿using MediatR;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Njoy.Admin.Features
{
    public sealed class EditAdminUserFeature
    {
        public sealed class Handler : IRequestHandler<Request, AdminUserRowModel>
        {
            private readonly UserManager<AdminUser> _userManager;

            public Handler(UserManager<AdminUser> userManager)
            {
                _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            }

            public async Task<AdminUserRowModel> Handle(Request request, CancellationToken cancellationToken)
            {
                if (!request.IsValid())
                {
                    throw new Exception("Request is not valid.");
                }

                var user = await _userManager.FindByIdAsync(request.Id);
                if (user is null)
                {
                    throw new Exception($"{nameof(AdminUser)} with Id of {request.Id} does not exist.");
                }

                // Update claims; FirstName, LastName
                var claims = await _userManager.GetClaimsAsync(user);
                UpdateClaim(user, claims, ClaimTypes.GivenName, request.FirstName);
                UpdateClaim(user, claims, ClaimTypes.Surname, request.LastName);

                if (!string.IsNullOrEmpty(request.NewPassword))
                {
                    // Update password
                    var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
                    ThrowIfFailed(result, "Updating password");
                }

                // Refresh claims
                claims = await _userManager.GetClaimsAsync(user);

                return new AdminUserRowModel
                {
                    Id = user.Id,
                    Username = user.UserName,
                    FirstName = claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value,
                    LastName = claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value,
                    Email = user.Email
                };
            }

            private async void UpdateClaim(AdminUser user, IEnumerable<Claim> claims, string claimType, string value)
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    var claim = claims.FirstOrDefault(c => c.Type == claimType);
                    if (claim != null)
                    {
                        var result = await _userManager.ReplaceClaimAsync(user, claim, new Claim(claimType, value));
                        ThrowIfFailed(result, $"Updating claim {claimType}");
                    }
                    else
                    {
                        var result = await _userManager.AddClaimAsync(user, new Claim(claimType, value));
                        ThrowIfFailed(result, $"Updating claim {claimType}");
                    }
                }
            }

            public void ThrowIfFailed(IdentityResult result, string operation)
            {
                if (!result.Succeeded)
                {
                    throw new Exception($"{operation} failed. Errors: {JsonConvert.SerializeObject(result.Errors)}");
                }
            }
        }

        public sealed class Request : IRequest<AdminUserRowModel>
        {
            [Required, MinLength(1)]
            public string Id { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            [EmailAddress]
            public string Email { get; set; }

            [MinLength(6)]
            public string CurrentPassword { get; set; }

            [MinLength(6)]
            public string NewPassword { get; set; }

            [MinLength(6)]
            public string NewPasswordConfirm { get; set; }

            public bool IsValid()
            {
                return NewPassword == NewPasswordConfirm
                    && (NewPassword != null ? CurrentPassword != null : true);
            }
        }
    }
}