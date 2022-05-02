using AgileIM.Shared.Models.ApiResult;
using AgileIM.Shared.Models.Users;

namespace AgileIM.Service.Services.UserService
{
    public interface IUserService
    {
        Task<User> Login(string account, string phone, string password);
    }
}
