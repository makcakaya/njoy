using System.Threading.Tasks;

namespace Njoy.Admin
{
    public interface IJwtService
    {
        Task<string> GenerateToken(string username, string password);
    }
}