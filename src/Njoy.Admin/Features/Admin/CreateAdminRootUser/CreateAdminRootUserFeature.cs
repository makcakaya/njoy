using MediatR;
using Njoy.Data;
using Njoy.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Njoy.Admin.Features
{
    public sealed class CreateAdminRootUserFeature
    {
        public sealed class Handler : AsyncRequestHandler<Request>
        {
            private readonly IUserService _userService;
            private readonly AdminRootConfig _adminRootConfig;

            public Handler(IUserService userService, AdminRootConfig adminRootConfig)
            {
                _userService = userService ?? throw new ArgumentNullException(nameof(userService));
                _adminRootConfig = adminRootConfig ?? throw new ArgumentNullException(nameof(adminRootConfig));
            }

            protected async override Task Handle(Request request, CancellationToken cancellationToken)
            {
                request = request ?? throw new ArgumentNullException(nameof(request));

                if (await _userService.DoesAdminRootExist()) { return; }

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