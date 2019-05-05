using FluentValidation;
using MediatR;
using Nensure;
using Njoy.Services;
using System.Threading;
using System.Threading.Tasks;

namespace Njoy.Admin
{
    public sealed class GetMerchantFeature
    {
        public sealed class Handler : IRequestHandler<Request, Response>
        {
            private readonly IMerchantService _merchantService;

            public Handler(IMerchantService merchantService)
            {
                Ensure.NotNull(merchantService);
                _merchantService = merchantService;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var result = await _merchantService.Get(request.MerchantId);
                return result != null ? new Response
                {
                    Id = result.Id,
                    UserId = result.UserId,
                    Username = result.User.UserName,
                    EmailAddress = result.User.Email
                } : null;
            }
        }

        public sealed class Request : AbstractValidator<Request>, IRequest<Response>
        {
            public int MerchantId { get; set; }

            public Request()
            {
                RuleFor(r => r.MerchantId).NotEmpty();
            }
        }

        public sealed class Response
        {
            public int Id { get; set; }
            public string UserId { get; set; }
            public string Username { get; set; }
            public string EmailAddress { get; set; }
        }
    }
}