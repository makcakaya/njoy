using MediatR;
using Nensure;
using Newtonsoft.Json;
using Njoy.Services;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Njoy.Admin.Features
{
    public sealed class EditAdminUserFeature
    {
        public sealed class Handler : IRequestHandler<Request>
        {
            private readonly IUserService _userService;

            public Handler(IUserService userService)
            {
                Ensure.NotNull(userService);
                _userService = userService;
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                Ensure.NotNull(request);
                await _userService.Edit(request.Map());
                return Unit.Value;
            }
        }

        public sealed class Request : IRequest<Unit>, IMapper<EditUserRequest>
        {
            [Required, MinLength(1)]
            public string Id { get; set; }

            [EmailAddress]
            public string Email { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            [MinLength(6)]
            public string CurrentPassword { get; set; }

            [MinLength(6)]
            public string NewPassword { get; set; }

            [MinLength(6)]
            public string NewPasswordConfirm { get; set; }

            public EditUserRequest Map()
            {
                var claims = new Dictionary<string, string>();
                if (FirstName != null)
                {
                    claims.Add(ClaimTypes.GivenName, FirstName);
                }
                if (LastName != null)
                {
                    claims.Add(ClaimTypes.Surname, LastName);
                }

                return new EditUserRequest
                {
                    Id = Id,
                    Email = Email,
                    Claims = claims,
                    CurrentPassword = CurrentPassword,
                    NewPassword = NewPassword,
                    NewPasswordConfirm = NewPasswordConfirm
                };
            }
        }
    }
}