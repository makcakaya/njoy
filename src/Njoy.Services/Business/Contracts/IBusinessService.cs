using Njoy.Data;
using System.Threading.Tasks;

namespace Njoy.Services
{
    public interface IBusinessService
    {
        Task<Business> Create(CreateBusinessParam createBusiness);

        Task<BusinessAddress> CreateAddress(CreateBusinessAddressParam address);
    }
}