using AgileIM.Service.Helper;
using AgileIM.Shared.Models.Im;

using System.Text.RegularExpressions;

namespace AgileIM.Service.Service
{
    public class ImService : IImService
    {
        public async Task<(bool, string)> CreateGroup(GroupInfo groupInfo)
        {
            return await ImHelper.Default.CreateGroup(groupInfo);
        }

        public async Task<(bool, string)> JoinOneGroup(string groupId, UserInfo userInfo)
        {
            return await ImHelper.Default.JoinGroup(groupId, userInfo);
        }

        public async Task<(bool, string)> JoinGroup(string groupId, List<UserInfo> userList)
        {
            return await ImHelper.Default.JoinGroup(groupId, userList);
        }

        public async Task<bool> DeleteGroup(string groupId)
        {
            return await ImHelper.Default.DeleteGroup(groupId);
        }

        public async Task<bool> ExitGroup(string groupId, string userId)
        {
            return await ImHelper.Default.ExitGroup(groupId, userId);
        }

        public async Task<string> SendMessage(Message message)
        {
            return await ImHelper.Default.SendMessage(message);
        }

        public async Task<(List<Message>?, string)> GetHistoryMessage(MsgType msgType, string targetId, string msgTime = "0-0")
        {
            if (string.IsNullOrEmpty(targetId)) return (null, "targetId为空");
            var channelStr = msgType switch
            {
                MsgType.One => $"{ImHelper.ONE}",
                MsgType.Group => $"{ImHelper.GROUP}_{ImHelper.HISTORY}_{targetId}",
                _ => string.Empty
            };
            if (string.IsNullOrEmpty(channelStr)) return (null, "MsgType错误");
            return (await ImHelper.Default.GetHistoryMessage(channelStr, msgTime), "成功");
        }
    }
}
