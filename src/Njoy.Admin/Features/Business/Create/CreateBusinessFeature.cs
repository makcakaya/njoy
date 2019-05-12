using MediatR;
using Nensure;
using Njoy.Services;
using System.Threading;
using System.Threading.Tasks;

namespace Njoy.Admin
{
    public sealed class CreateBusinessFeature
    {
        public sealed class Handler : IRequestHandler<Request, Response>
        {
            private readonly IBusinessService _businessService;

            public Handler(IBusinessService businessService)
            {
                Ensure.NotNull(businessService);
                _businessService = businessService;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                Ensure.NotNull(request);
                var business = await _businessService.Create(request);
                return new Response
                {
                    BusinessId = business.Id
                };
            }
        }

        public sealed class Request : CreateBusinessParam, IRequest<Response>
        {
        }

        public sealed class Response
        {
            public int BusinessId { get; set; }
        }
    }
}