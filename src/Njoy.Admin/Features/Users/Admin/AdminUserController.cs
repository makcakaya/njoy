using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nensure;
using Njoy.Admin.Features;
using Njoy.Data;
using System.Threading.Tasks;

namespace Njoy.Admin
{
    [ApiController, Route(Route)]
    [Authorize(Roles = AppRole.AdminRoot)]
    public sealed class AdminUserController : ControllerBase
    {
        public const string Route = "api/adminuser";

        private readonly IMediator _mediator;

        public AdminUserController(IMediator mediator)
        {
            Ensure.NotNull(mediator);
            _mediator = mediator;
        }

        [HttpPost, Route("create")]
        public async Task<AdminUserRowModel> Create(CreateAdminUserFeature.Request request)
        {
            return await _mediator.Send(request);
        }

        [HttpPost, Route("update")]
        public async Task Update(EditAdminUserFeature.Request request)
        {
            await _mediator.Send(request);
        }

        [HttpGet, Route("list")]
        public async Task<ListAdminUsersFeature.Response> List(ListAdminUsersFeature.Request request)
        {
            return await _mediator.Send(request);
        }

        [HttpPost, Route("login")]
        [AllowAnonymous]
        public async Task<string> Login(LoginAdminUserFeature.Request request)
        {
            return await _mediator.Send(request);
        }
    }
}