using Njoy.Data;

namespace Njoy.Services
{
    public interface IBusinessService
    {
        Business Create(CreateBusinessParam createBusiness);
    }
}