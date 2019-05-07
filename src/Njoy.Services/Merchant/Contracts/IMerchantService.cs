using Njoy.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Njoy.Services
{
    public interface IMerchantService
    {
        Task<IEnumerable<Merchant>> Search(string username);

        Task<Merchant> Get(int id);
    }
}