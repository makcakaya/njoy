using MediatR;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Njoy.Admin.Features
{
    public sealed class LoginAdminUserFeature
    {
        public sealed class Handler : IRequestHandler<Request, string>
        {
            private readonly IJwtService _jwtService;

            public Handler(IJwtService jwtService)
            {
                _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
            }

            public async Task<string> Handle(Request request, CancellationToken cancellationToken)
            {
                request = request ?? throw new ArgumentNullException(nameof(request));

                return await _jwtService.GenerateToken(request.Username, request.Password);
            }
        }

        public sealed class Request : IRequest<string>
        {
            [Required]
            public string Username { get; set; }

            [Required]
            public string Password { get; set; }
        }
    }
}