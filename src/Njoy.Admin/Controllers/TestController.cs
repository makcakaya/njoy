using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Njoy.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class TestController : ControllerBase
    {
        [HttpGet("[action]")]
        public string Test()
        {
            return "hello World";
        }

        [HttpGet("[action]")]
        [Authorize]
        public string TestAuth()
        {
            return "hello World";
        }
    }
}