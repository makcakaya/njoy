﻿using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Nensure;
using Njoy.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Njoy.Services
{
    public sealed class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly NjoyContext _context;

        public UserService(NjoyContext context, UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager)
        {
            Ensure.NotNull(context, userManager, roleManager);
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<CreateUserResponse> Create(CreateUserRequest param)
        {
            Ensure.NotNull(param);
            param.ValidateAndThrow(param);
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                if (DoesUserNameExist(param.Username))
                {
                    throw new OperationFailedException($"{nameof(AppUser)} with username {param.Username} already exist.");
                }

                var user = new AppUser
                {
                    UserName = param.Username,
                    Email = param.Email,
                };

                IdentityAssert.ThrowIfFailed(
                    await _userManager.CreateAsync(user, param.Password),
                    nameof(_userManager.CreateAsync));

                await AddClaim(user, ClaimTypes.GivenName, param.FirstName);
                await AddClaim(user, ClaimTypes.Surname, param.LastName);

                IdentityAssert.ThrowIfFailed(
                    await _userManager.AddToRoleAsync(user, param.Role),
                    nameof(_userManager.AddToRoleAsync));

                transaction.Commit();

                return new CreateUserResponse { User = user };
            }
        }

        public bool DoesUserNameExist(string username)
        {
            Ensure.NotNullOrWhitespace(username);
            return _userManager.Users.Any(u => u.UserName == username);
        }

        public async Task Edit(EditUserRequest request)
        {
            Ensure.NotNull(request);
            request.ValidateAndThrow(request);
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                var user = await _userManager.FindByIdAsync(request.Id);
                if (user is null)
                {
                    throw new InvalidOperationException($"{nameof(AppUser)} with Id of {request.Id} does not exist.");
                }

                // Update claims; FirstName, LastName
                var existingClaims = await _userManager.GetClaimsAsync(user);
                foreach (var claim in request.Claims)
                {
                    UpdateClaim(user, existingClaims, claim.Key, claim.Value);
                }

                if (!string.IsNullOrEmpty(request.NewPassword))
                {
                    // Update password
                    var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
                    IdentityAssert.ThrowIfFailed(result, nameof(_userManager.ChangePasswordAsync));
                }

                transaction.Commit();
            }
        }

        public async Task<GetUsersResponse> Get(GetUsersRequest request)
        {
            Ensure.NotNull(request);
            var result = new List<AppUser>();
            foreach (var role in request.Roles)
            {
                result.AddRange(await _userManager.GetUsersInRoleAsync(role));
            }

            return new GetUsersResponse
            {
                Users = result.Select(u => new GetUsersResponse.Record
                {
                    Id = u.Id,
                    Username = u.UserName,
                    Email = u.Email
                })
            };
        }

        private async void UpdateClaim(AppUser user, IEnumerable<Claim> claims, string claimType, string value)
        {
            Ensure.NotNull(user);
            if (!string.IsNullOrWhiteSpace(value))
            {
                var claim = claims.FirstOrDefault(c => c.Type == claimType);
                if (claim != null)
                {
                    var result = await _userManager.ReplaceClaimAsync(user, claim, new Claim(claimType, value));
                    IdentityAssert.ThrowIfFailed(result, $"Updating claim {claimType}");
                }
                else
                {
                    await AddClaim(user, claimType, value);
                }
            }
        }

        private async Task AddClaim(AppUser user, string claimType, string value)
        {
            Ensure.NotNull(user);
            if (!string.IsNullOrWhiteSpace(value))
            {
                var result = await _userManager.AddClaimAsync(user, new Claim(claimType, value));
                IdentityAssert.ThrowIfFailed(result, $"Adding claim {claimType}");
            }
        }
    }
}