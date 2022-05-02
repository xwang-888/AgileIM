using AgileIM.Shared.Models.ApiResult;

using Microsoft.AspNetCore.Mvc;

using AgileIM.IM.Models;
using AgileIM.Service.Services;

namespace AgileIM.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImController : ControllerBase
    {
        private readonly IImService _imService;
        public ImController(IImService imService)
        {
            _imService = imService;
        }

        /// <summary>
        /// 创建群组
        /// </summary>
        /// <returns></returns>
        [HttpPost("CreateGroup")]
        public async Task<Result> CreateGroup(GroupInfo groupInfo)
        {
            var (item1, item2) = await _imService.CreateGroup(groupInfo);
            return item1 ? new Result("200", "创建成功") : new Result("500", item2);
        }
        /// <summary>
        /// 单个人加入群组
        /// </summary>
        /// <returns></returns>
        [HttpPost("JoinOneGroup")]
        public async Task<Result> JoinOneGroup(string groupId, UserInfo userInfo)
        {
            var (item1, item2) = await _imService.JoinOneGroup(groupId, userInfo);
            return item1 ? new Result("200", "加入成功") : new Result("500", item2);
        }
        /// <summary>
        /// 加入群组
        /// </summary>
        /// <returns></returns>
        [HttpPost("JoinGroup")]
        public async Task<Result> JoinGroup(string groupId, List<UserInfo> userInfos)
        {
            var (item1, item2) = await _imService.JoinGroup(groupId, userInfos);
            return item1 ? new Result("200", "加入成功") : new Result("500", item2);
        }
        /// <summary>
        /// 删除群组
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [HttpDelete("DeleteGroup")]
        public async Task<Result> DeleteGroup(string groupId)
            => await _imService.DeleteGroup(groupId) ? new Result("200", "删除成功") : new Result("500", "删除失败");
        /// <summary>
        /// 离开群组
        /// </summary>
        /// <param name="groupId">群组id</param>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        [HttpGet("ExitGroup")]
        public async Task<Result> ExitGroup(string groupId, string userId)
            => await _imService.ExitGroup(groupId, userId) ? new Result("200", "离开成功") : new Result("500", "离开失败");
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message">消息对象</param>
        /// <returns></returns>
        [HttpPost("SendMessage")]
        public async Task<Result<string>> SendMessage(Message message)
        {
            var endMessageStr = await _imService.SendMessage(message);
            return new Result<string>("200", "发送成功", endMessageStr);
        }
        /// <summary>
        /// 获取历史记录
        /// </summary>
        /// <param name="msgType">消息类型 1=单聊，2=群组</param>
        /// <param name="targetId">获取方Id</param>
        /// <param name="msgTime">时间戳</param>
        /// <returns></returns>
        [HttpGet("GetHistoryMessage")]
        public async Task<Result<List<Message>?>> GetHistoryMessage(MsgType msgType, string targetId = null, string msgTime = "0-0")
        {
            var (msgList, msg) = await _imService.GetHistoryMessage(msgType, targetId, msgTime);
            return msgList is not null
                ? new Result<List<Message>?>("200", msg, msgList)
                : new Result<List<Message>?>("500", msg, null);
        }
    }
}
