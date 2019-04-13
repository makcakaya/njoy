using System.Collections.Generic;

namespace Njoy.Services
{
    public sealed class GetUsersRequest
    {
        public IEnumerable<string> Roles { get; set; } = new List<string>();
    }
}