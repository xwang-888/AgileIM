using AgileIM.IM.Models;

namespace AgileIM.Service.Service
{
    public interface IImService
    {
        /// <summary>
        /// 创建组
        /// </summary>
        /// <param name="groupInfo"></param>
        Task<(bool, string)> CreateGroup(GroupInfo groupInfo);
        /// <summary>
        /// 单个人加入组
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="userInfo"></param>
        Task<(bool, string)> JoinOneGroup(string groupId, UserInfo userInfo);
        /// <summary>
        /// 多人加入组
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="userList"></param>
        Task<(bool, string)> JoinGroup(string groupId, List<UserInfo> userList);
        /// <summary>
        /// 删除组
        /// </summary>
        /// <param name="groupId"></param>
        Task<bool> DeleteGroup(string groupId);
        /// <summary>
        /// 退出群组
        /// </summary>
        Task<bool> ExitGroup(string groupId, string userId);
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task<string> SendMessage(Message message);
        /// <summary>
        /// 获取历史记录
        /// </summary>
        /// <param name="msgType"></param>
        /// <param name="targetId"></param>
        /// <param name="msgTime"></param>
        /// <returns></returns>
        Task<(List<Message>?, string)> GetHistoryMessage(MsgType msgType, string targetId, string msgTime = "0-0");
    }
}
