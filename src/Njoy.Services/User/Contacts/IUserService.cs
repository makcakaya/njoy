using System.Threading.Tasks;

namespace Njoy.Services
{
    public interface IUserService
    {
        Task<CreateUserResponse> Create(CreateUserRequest param);

        bool DoesUserNameExist(string username);

        Task<bool> DoesAdminRootExist();
    }
}