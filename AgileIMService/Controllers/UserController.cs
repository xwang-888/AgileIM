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

        [HttpPost("Login")]
        public async Task<Result<LoginUserDto?>> Login([FromBody] LoginRequest loginRequest)
        {
            var result = await _userService.Login(loginRequest.UserAccountOrMobile, loginRequest.PassWord);

            if (result is not null)
                await _userService.UpdateLastDateTime(result.UserUid);


            return result is null ?
                new Result<LoginUserDto?>(201, "账户名密码错误", null) :
                new Result<LoginUserDto?>(200, "登录成功", result);
        }

        /// <summary>
        /// 刷新token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        [HttpPost("RefreshToken")]
        [AllowAnonymous]
        public async Task<Result<RefreshTokenDto>> RefreshToken(string refreshToken)
        {
            var result = new Result<RefreshTokenDto>();
            if (string.IsNullOrEmpty(refreshToken?.Trim()))
            {
                result.Msg = "参数丢失 refreshToken";
                result.Code = 201;
                return result;
            }

            var refreshTokenDto = await _userService.RefreshToken(refreshToken);

            if (refreshTokenDto is null)
            {
                result.Msg = "获取token错误";
                result.Code = 201;
            }
            else
            {
                result.Msg = "成功";
                result.Code = 200;
                result.Data = refreshTokenDto;
            }

            return result;
        }
    }
}
