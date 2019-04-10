using System.Threading.Tasks;

namespace Njoy.Services
{
    public interface IUserService
    {
        Task<CreateUserResponse> Create(CreateUserRequest request);

        bool DoesUserNameExist(string username);

        Task<bool> DoesAdminRootExist();

        Task Edit(EditUserRequest request);
    }
}