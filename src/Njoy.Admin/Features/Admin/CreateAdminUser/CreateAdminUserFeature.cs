using MediatR;
using Newtonsoft.Json;
using Njoy.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Njoy.Admin.Features
{
    public sealed class CreateAdminUserFeature
    {
        public sealed class Handler : IRequestHandler<Request, AdminUserRowModel>
        {
            private readonly IUserService _userService;

            public Handler(IUserService userService)
            {
                _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            }

            public async Task<AdminUserRowModel> Handle(Request request, CancellationToken cancellationToken)
            {
                if (!request.IsValid())
                {
                    throw new ArgumentException("Request is not valid.");
                }

                var response = await _userService.Create(request.Map());
                return new AdminUserRowModel
                {
                    Id = response.Id
                };
            }
        }

        public sealed class Request : IRequest<AdminUserRowModel>, IMapper<CreateUserRequest>
        {
            public string Id { get; set; }

            [Required]
            public string Username { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            [EmailAddress]
            public string Email { get; set; }

            [MinLength(6)]
            public string Password { get; set; }

            [MinLength(6)]
            public string PasswordConfirm { get; set; }

            [Required]
            public string Role { get; set; }

            public bool IsValid()
            {
                return Password != null && Password == PasswordConfirm;
            }

            public CreateUserRequest Map()
            {
                return new CreateUserRequest
                {
                    Username = Username,
                    Email = Email,
                    Password = Password,
                    PasswordConfirm = PasswordConfirm,
                    FirstName = FirstName,
                    LastName = LastName,
                    Role = Role
                };
            }
        }
    }
}