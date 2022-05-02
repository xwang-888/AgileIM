using AgileIM.Shared.EFCore;
using AgileIM.Shared.Models.Users;

using Microsoft.EntityFrameworkCore;

namespace AgileIM.Service.Services.UserService
{
    public class UserService : IUserService
    {
        public UserService(AgileImDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private readonly AgileImDbContext _dbContext;

        public async Task<User> Login(string account, string phone, string password)
        {

            var user = await _dbContext.Users.FirstOrDefaultAsync(u =>
                   (u.Account.Equals(account) || u.Photo.Equals(phone)) && password.Equals(password));

            return user;
        }
    }
}
