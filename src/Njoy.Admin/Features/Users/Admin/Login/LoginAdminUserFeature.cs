using MediatR;
using Nensure;
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
                Ensure.NotNull(jwtService);
                _jwtService = jwtService;
            }

            public async Task<string> Handle(Request request, CancellationToken cancellationToken)
            {
                Ensure.NotNull(request);
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