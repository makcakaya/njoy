using MediatR;
using Microsoft.AspNetCore.Identity;
using Nensure;
using Njoy.Data;
using Njoy.Services;
using System.Threading;
using System.Threading.Tasks;

namespace Njoy.Admin.Features
{
    public sealed class CreateAdminRootUserFeature
    {
        public sealed class Handler : AsyncRequestHandler<Request>
        {
            private readonly IUserService _userService;
            private readonly UserManager<AppUser> _userManager;
            private readonly AdminRootConfig _adminRootConfig;

            public Handler(IUserService userService, UserManager<AppUser> userManager, AdminRootConfig adminRootConfig)
            {
                Ensure.NotNull(userService).NotNull(userManager).NotNull(adminRootConfig);
                _userService = userService;
                _userManager = userManager;
                _adminRootConfig = adminRootConfig;
            }

            protected async override Task Handle(Request request, CancellationToken cancellationToken)
            {
                Ensure.NotNull(request);
                if (!_adminRootConfig.AllowMultipleRootUsers)
                {
                    var existingUsers = await _userManager.GetUsersInRoleAsync(AppRole.AdminRoot);
                    if (existingUsers.Count > 0) { return; }
                }

                var existingUser = await _userManager.FindByNameAsync(_adminRootConfig.Username);
                if (existingUser != null)
                {
                    var roles = await _userManager.GetRolesAsync(existingUser);
                    if (roles.Contains(AppRole.AdminRoot))
                    {
                        return;
                    }
                    else
                    {
                        throw new OperationFailedException(nameof(CreateAdminRootUserFeature),
                            $"A user with username {_adminRootConfig.Username} already exists and does not have {AppRole.AdminRoot} role.");
                    }
                }

                await _userService.Create(
                    new CreateUserRequest
                    {
                        Username = _adminRootConfig.Username,
                        Password = _adminRootConfig.Password,
                        PasswordConfirm = _adminRootConfig.Password,
                        Email = _adminRootConfig.Email,
                        FirstName = _adminRootConfig.FirstName,
                        LastName = _adminRootConfig.LastName,
                        Role = AppRole.AdminRoot
                    });
            }
        }

        /// <summary>
        /// Does not need any properties as AdminRoot user has predefined values in the configuration.
        /// </summary>
        public sealed class Request : IRequest
        {
        }
    }
}