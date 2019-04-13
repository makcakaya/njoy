using System.Collections.Generic;

namespace Njoy.Services
{
    public sealed class GetUsersResponse
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