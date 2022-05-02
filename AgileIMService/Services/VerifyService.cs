using AgileIM.Shared.Models.Users;

namespace AgileIM.Service.Services
{
    public class VerifyService : IVerifyService
    {
        public async Task<User?> VerifyUser(string account, string password)
        {
            if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(password)) return null;


            //TODO 此处应为从数据库查询数据，来判断是否登录成功
            return new User
            {
                Id = $"{Guid.NewGuid()}",
                Account = account,
                Password = password,
                Nick = $"尼古拉斯{new Random().Next(1000, 10000)}"
            };
        }
    }
}
