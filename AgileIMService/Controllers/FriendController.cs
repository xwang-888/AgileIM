using AgileIM.Service.Services.UserService;
using AgileIM.Shared.Models.ApiResult;
using AgileIM.Shared.Models.Users.Entity;
using AgileIM.Shared.Models.Users.Request;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgileIM.Service.Controllers
{
    [Route("api/[controller]")]
    public class FriendController : ControllerBase
    {

        public FriendController(IFriendService friendService)
        {
            _friendService = friendService;
        }
        private readonly IFriendService _friendService;
        [HttpPost("Add")]
        [Authorize]
        public async Task<Response<Friend>> Add([FromBody] FriendRequest friend)
        {
            var model = await _friendService.InsertAsync(new Friend() { UserId = friend.UserId, FriendId = friend.FriendId });

            return model is not null ?
                new Response<Friend>(200, "成功", model) :
                new Response<Friend>(201, "失败", null);
        }
        [HttpDelete("Delete")]
        [Authorize]
        public async Task<Response<bool>> Delete([FromBody] FriendRequest friend)
        {
            var isOk = await _friendService.DeleteFriendAsync(friend.UserId, friend.FriendId);

            return isOk ?
                new Response<bool>(200, "成功", true) :
                new Response<bool>(201, "失败", false);
        }
        [HttpPost("GetFriendListByUserId")]
        [Authorize]
        public async Task<Response<IEnumerable<User>?>> GetFriendListByUserId([FromBody] FriendRequest friendRequest)
        {
            var model = await _friendService.GetFriendListByUserIdAsync(friendRequest.UserId);

            return model is not null ?
                new Response<IEnumerable<User>?>(200, "成功", model) :
                new Response<IEnumerable<User>?>(201, "失败", null);
        }

    }
}
