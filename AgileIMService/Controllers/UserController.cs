using AgileIM.Service.Services.UserService;
using AgileIM.Shared.Models.ApiResult;
using AgileIM.Shared.Models.Users;

using Microsoft.AspNetCore.Mvc;

namespace AgileIM.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        private readonly IUserService _userService;

        [HttpPost("Login")]
        public async Task<Result> Login([FromBody] User user)
        {
            var result = await _userService.Login(user.Account, user.Photo, user.Password);
            if (result is null)
                return new Result("201", "账户名密码错误");
            else
                return new Result("200", "登录成功");

        }
    }
}
