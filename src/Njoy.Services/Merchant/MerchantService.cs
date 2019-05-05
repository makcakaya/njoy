using Microsoft.EntityFrameworkCore;
using Nensure;
using Njoy.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Njoy.Services
{
    public sealed class MerchantService : IMerchantService
    {
        private readonly NjoyContext _context;

        public MerchantService(NjoyContext context)
        {
            Ensure.NotNull(context);
            _context = context;
        }

        public async Task<Merchant> Get(int id)
        {
            return await _context.Set<Merchant>().FindAsync(id);
        }

        public async Task<IEnumerable<Merchant>> Search(string username)
        {
            return await _context.Set<Merchant>()
                .Where(m => m.User.NormalizedUserName.Contains(username.ToUpper()))
                .ToArrayAsync();
        }
    }
}