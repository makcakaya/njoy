using FluentValidation;
using MediatR;
using Njoy.Data;
using Njoy.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Njoy.Admin
{
    public sealed class CreateMerchantUserFeature
    {
        public sealed class Handler : IRequestHandler<Request>
        {
            private readonly IUserService _userService;

            public Handler(IUserService userService)
            {
                _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                if (request is null) { throw new ArgumentNullException(nameof(request)); }

                //_userService.Create();

                throw new System.NotImplementedException();
            }
        }

        public sealed class Request : AbstractValidator<Request>, IRequest, IUserRegistrationModel
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string PasswordConfirm { get; set; }
            public string BusinessId { get; set; }

            public Request()
            {
                RuleFor(r => r.Username).MinimumLength(UserConfig.MinUsernameLength);
                RuleFor(r => r.Email).EmailAddress();
                RuleFor(r => r.Password).MinimumLength(UserConfig.MinPasswordLength).Equal(r => r.PasswordConfirm);
                RuleFor(r => r.BusinessId).NotEmpty();
            }
        }
    }
}