using MediatR;
using Newtonsoft.Json;
using Njoy.Services;
using System;
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
                _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                if (!request.IsValid())
                {
                    throw new ArgumentException("Request is not valid.");
                }

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

            public bool IsValid()
            {
                return NewPassword == NewPasswordConfirm
                    && (NewPassword != null ? CurrentPassword != null : true);
            }

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