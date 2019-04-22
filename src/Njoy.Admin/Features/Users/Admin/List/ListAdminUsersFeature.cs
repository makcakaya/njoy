using MediatR;
using Nensure;
using Njoy.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Njoy.Admin.Features
{
    public sealed class ListAdminUsersFeature
    {
        public sealed class Handler : IRequestHandler<Request, Response>
        {
            private readonly IUserService _userService;

            public Handler(IUserService userService)
            {
                Ensure.NotNull(userService);
                _userService = userService;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                Ensure.NotNull(request);
                var result = await _userService.Get(request.Map());
                return new Response
                {
                    Users = result.Users.Select(u => new Response.Record
                    {
                        Id = u.Id,
                        Username = u.Username,
                        Email = u.Email
                    })
                };
            }
        }

        public sealed class Request : IRequest<Response>, IMapper<GetUsersRequest>
        {
            public IEnumerable<string> Roles { get; set; }

            public GetUsersRequest Map()
            {
                return new GetUsersRequest { Roles = Roles };
            }
        }

        public sealed class Response
        {
            public IEnumerable<Record> Users { get; set; }

            public sealed class Record
            {
                public string Id { get; set; }
                public string Username { get; set; }
                public string Email { get; set; }
            }
        }
    }
}