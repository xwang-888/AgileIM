using AgileIM.Service.Services.UserService;
using AgileIM.Shared.Models.ApiResult;
using AgileIM.Shared.Models.Users;
using AgileIM.Shared.Models.Users.Dto;
using AgileIM.Shared.Models.Users.Entity;
using AgileIM.Shared.Models.Users.Request;

using Microsoft.AspNetCore.Authorization;
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
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public async Task<Response<LoginUserDto?>> Login([FromBody] LoginRequest loginRequest)
        {
            var result = await _userService.Login(loginRequest.UserAccountOrMobile, loginRequest.PassWord);

            if (result is not null)
                await _userService.UpdateLastDateTime(result.Id);


            return result is null ?
                new Response<LoginUserDto?>(201, "账户名密码错误", null) :
                new Response<LoginUserDto?>(200, "登录成功", result);
        }

        /// <summary>
        /// 刷新token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        [HttpPost("RefreshToken")]
        [AllowAnonymous]
        public async Task<Response<RefreshTokenDto>> RefreshToken(string refreshToken)
        {
            var result = new Response<RefreshTokenDto>();
            if (string.IsNullOrEmpty(refreshToken?.Trim()))
            {
                result.Message = "参数丢失 refreshToken";
                result.Code = 201;
                return result;
            }

            var refreshTokenDto = await _userService.RefreshToken(refreshToken);

            if (refreshTokenDto is null)
            {
                result.Message = "获取token错误";
                result.Code = 201;
            }
            else
            {
                result.Message = "成功";
                result.Code = 200;
                result.Data = refreshTokenDto;
            }

            return result;
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("Register")]
        public async Task<Response<User>> Register([FromBody] User user)
        {
            var resultUser = await _userService.InsertAsync(user);

            return resultUser is not null ?
                new Response<User>(200, "注册成功", resultUser) :
                new Response<User>(201, "注册失败", null);
        }

        [HttpGet("QueryFriends")]
        [Authorize]
        public async Task<Response<IEnumerable<User>>> QueryFriends(string userAccountOrMobile)
        {
            var model = await _userService.QueryFriends(userAccountOrMobile);

            return model is not null ?
                new Response<IEnumerable<User>>(200, "成功", model) :
                new Response<IEnumerable<User>>(201, "失败", null);
        }

    }
}
