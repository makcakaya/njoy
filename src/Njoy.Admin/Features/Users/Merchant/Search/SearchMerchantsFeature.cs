using FluentValidation;
using MediatR;
using Nensure;
using Njoy.Data;
using Njoy.Services;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Njoy.Admin
{
    public sealed class SearchMerchantsFeature
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
                var result = await _merchantService.Search(request.SearchPhrase);
                return ToResponse(result);
            }

            private Response ToResponse(IEnumerable<Merchant> merchants)
            {
                var rows = new List<Response.Row>();
                foreach (var merchant in merchants)
                {
                    rows.Add(new Response.Row
                    {
                        Id = merchant.Id,
                        UserId = merchant.UserId,
                        Username = merchant.User.UserName
                    });
                }
                return new Response { Merchants = rows.ToArray() };
            }
        }

        public sealed class Request : AbstractValidator<Request>, IRequest<Response>
        {
            public string SearchPhrase { get; set; }

            public Request()
            {
                RuleFor(r => r.SearchPhrase).NotEmpty();
            }
        }

        public sealed class Response
        {
            public Row[] Merchants { get; set; }

            public sealed class Row
            {
                public int Id { get; set; }
                public string UserId { get; set; }
                public string Username { get; set; }
            }
        }
    }
}