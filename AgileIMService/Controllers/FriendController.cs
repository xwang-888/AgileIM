using AgileIM.Service.Services.FriendService;
using AgileIM.Service.Services.UserService;
using AgileIM.Shared.Models.ApiResult;
using AgileIM.Shared.Models.Friend.Entity;
using AgileIM.Shared.Models.Friend.Request;
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
        public async Task<Response<Friend?>> Add([FromBody] FriendRequest friend)
        {
            Friend? model = null;
            if (!await _friendService.ExistFriend(friend.UserId, friend.FriendId))
                model = await _friendService.InsertAsync(new Friend() { UserId = friend.UserId, FriendId = friend.FriendId });
            else
                return new Response<Friend?>(201, "好友已存在", null);

            return model is not null ?
                new Response<Friend?>(200, "成功", model) :
                new Response<Friend?>(201, "失败", null);
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

        [HttpPost("UpdateUserNote")]
        [Authorize]
        public async Task<Response<string?>> UpdateUserNote([FromBody] UpdateUserNoteRequest update)
        {
            var model = await _friendService.UpdateUserNote(update.UserId, update.FriendId, update.UserNote);

            return model is not null ?
                new Response<string?>(200, "成功", model.UserNote) :
                new Response<string?>(201, "失败", null);
        }
        [HttpGet("GetFriendListByUserId")]
        [Authorize]
        public async Task<Response<IEnumerable<Friend>?>> GetFriendListByUserId(string userId)
        {
            var model = await _friendService.GetFriendListByUserIdAsync(userId);

            return model is not null ?
                new Response<IEnumerable<Friend>?>(200, "成功", model) :
                new Response<IEnumerable<Friend>?>(201, "失败", null);
        }

    }
}
