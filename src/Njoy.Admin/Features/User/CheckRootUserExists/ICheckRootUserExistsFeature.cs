using System.Threading.Tasks;

namespace Njoy.Admin
{
    public interface ICheckRootUserExistsFeature
    {
        Task<bool> Run();
    }
}