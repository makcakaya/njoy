using System.Threading.Tasks;

namespace Njoy.Services
{
    public interface IUserService
    {
        Task<CreateUserResponse> Create(CreateUserRequest request);

        bool DoesUserNameExist(string username);

        Task Edit(EditUserRequest request);

        Task<GetUsersResponse> Get(GetUsersRequest request);
    }
}