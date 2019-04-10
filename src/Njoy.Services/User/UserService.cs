using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Njoy.Data;
using System;
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
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        }

        public async Task<CreateUserResponse> Create(CreateUserRequest param)
        {
            param = param ?? throw new ArgumentNullException(nameof(param));
            param.ValidateAndThrow(param);

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                if (DoesUserNameExist(param.Username))
                {
                    throw new OperationFailedException(
                        $"{nameof(AppUser)} with username {param.Username} already exist.");
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

                // TODO: Create predefined roles on startup
                //if (!await _roleManager.RoleExistsAsync(AppRole.Sales))
                //{
                //    IdentityAssert.ThrowIfFailed(await _roleManager.CreateAsync(new AppRole { Name = AppRole.Sales }), "Create Sales role");
                //}

                IdentityAssert.ThrowIfFailed(
                    await _userManager.AddToRoleAsync(user, param.Role),
                    nameof(_userManager.AddToRoleAsync));

                transaction.Commit();

                return new CreateUserResponse { Id = user.Id };
            }
        }

        public bool DoesUserNameExist(string username)
        {
            username = username ?? throw new ArgumentNullException(nameof(username));

            return _userManager.Users.Any(u => u.UserName == username);
        }

        public async Task<bool> DoesAdminRootExist()
        {
            return (await _userManager.GetUsersInRoleAsync(AppRole.AdminRoot)).Count() > 0;
        }

        private async Task AddClaim(AppUser user, string claimType, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                var result = await _userManager.AddClaimAsync(user, new Claim(claimType, value));
                IdentityAssert.ThrowIfFailed(result, $"Adding claim {claimType}");
            }
            return;
        }
    }
}