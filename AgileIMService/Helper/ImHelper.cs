using AgileIM.Shared.Models.Im;

using Newtonsoft.Json;

using StackExchange.Redis;

namespace AgileIM.Service.Helper
{
    public class ImHelper
    {

        private ImHelper() { }
        #region Consts
        /// <summary>
        /// 单聊
        /// </summary>
        public const string ONE = "Agile_IM_One";
        /// <summary>
        /// 群聊拼接+群ID
        /// </summary>
        public const string GROUP = "Agile_IM_Group";
        /// <summary>
        /// 订阅单聊
        /// </summary>
        public const string SUBSCRIBE = "Agile_IM_Subscribe";
        /// <summary>
        /// 订阅群聊
        /// </summary>
        public const string SUBSCRIBEGROUP = "Agile_IM_Group_Subscribe";
        /// <summary>
        /// 历史记录
        /// </summary>
        public const string HISTORY = "HISTORY";
        /// <summary>
        /// 单聊消息缓存最大数量
        /// </summary>
        private const int MAX_LENGTH_ONE = 1000;
        /// <summary>
        /// 群聊消息缓存最大数量
        /// </summary>
        private const int MAX_LENGTH_GROUP = 1000;
        #endregion

        #region Instance
        private static readonly object InstanceLock = new object();

        private static ImHelper? _default;
        public static ImHelper Default
        {
            get
            {
                if (_default is not null) return _default;
                lock (InstanceLock)
                {
                    _default ??= new ImHelper();
                }
                return _default;
            }
        }
        #endregion

        /// <summary>
        /// 创建群组
        /// </summary>
        /// <param name="groupInfo"></param>
        public async Task<(bool, string)> CreateGroup(GroupInfo groupInfo)
        {
            try
            {
                var redis = RedisManager.redisManager.GetDatabase();
                // 判断此群组是否存在，不存在则创建
                if (await redis.HashExistsAsync(GROUP, groupInfo.Id)) return (true, "群组已存在");

                await redis.HashSetAsync(GROUP, groupInfo.Id, groupInfo.Name);
                foreach (var item in groupInfo.UserInfos)
                {
                    if (!await redis.HashExistsAsync($"{GROUP}_{groupInfo.Id}", item.Id))
                        await redis.HashSetAsync($"{GROUP}_{groupInfo.Id}", item.Id, item.Name);
                }
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }

            return (true, string.Empty);
        }
        /// <summary>
        /// 加入群组
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="userInfo"></param>
        public async Task<(bool, string)> JoinGroup(string groupId, UserInfo userInfo)
        {
            var redis = RedisManager.redisManager.GetDatabase();
            // 判断此群组是否存在，不存在则创建
            if (!await redis.KeyExistsAsync($"{GROUP}_{groupId}"))
                return (false, "群组不存在");

            await redis.HashSetAsync($"{GROUP}_{groupId}", userInfo.Id, userInfo.Name);

            return (true, string.Empty);
        }
        /// <summary>
        /// 加入群组
        /// </summary>
        /// <param name="groupId">群组Id</param>
        /// <param name="userInfos"></param>
        public async Task<(bool, string)> JoinGroup(string groupId, List<UserInfo> userInfos)
        {
            var redis = RedisManager.redisManager.GetDatabase();

            // 判断此群组是否存在
            if (!await redis.KeyExistsAsync($"{GROUP}_{groupId}"))
                return (false, "群组不存在");
            foreach (var userInfo in userInfos)
            {
                await redis.HashSetAsync($"{GROUP}_{groupId}", userInfo.Id, userInfo.Name);
            }
            return (true, string.Empty);
        }
        /// <summary>
        /// 删除群组
        /// </summary>
        public async Task<bool> DeleteGroup(string groupId)
        {
            var redis = RedisManager.redisManager.GetDatabase();
            if (await redis.KeyExistsAsync(ImHelper.GROUP))
                await redis.HashDeleteAsync(ImHelper.GROUP, groupId);

            if (await redis.KeyExistsAsync($"{GROUP}_{groupId}"))
                return await redis.KeyDeleteAsync($"{GROUP}_{groupId}");
            return false;
        }
        /// <summary>
        /// 退出群组
        /// </summary>
        public async Task<bool> ExitGroup(string groupId, string userId)
        {
            var redis = RedisManager.redisManager.GetDatabase();

            if (await redis.HashExistsAsync($"{GROUP}_{groupId}", userId))
                return await redis.HashDeleteAsync($"{GROUP}_{groupId}", userId);

            return false;
        }
        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="subChannel"></param>
        /// <param name="handler"></param>
        public void Subscribe(string subChannel, Action<RedisChannel, RedisValue> handler)
        {
            var sub = RedisManager.redisManager.GetSubscriber();
            sub.Subscribe(subChannel, (channel, message) =>
            {
                //接收订阅消息
                if (handler == null)
                    Console.WriteLine(subChannel + " 订阅收到消息：" + message);
                else
                    handler(channel, message);
            });
        }
        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="subChannel"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public async Task UnSubscribe(string subChannel, Action<RedisChannel, RedisValue> handler = null)
        {
            var sub = RedisManager.redisManager.GetSubscriber();
            await sub.UnsubscribeAsync(subChannel, handler);
        }
        /// <summary>
        /// 取消全部订阅
        /// </summary>
        /// <param name="subChannel"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public async Task UnAllSubscribe(string subChannel, Action<RedisChannel, RedisValue> handler = null)
        {
            var sub = RedisManager.redisManager.GetSubscriber();
            await sub.UnsubscribeAllAsync();
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<string> SendMessage(Message message)
        {
            /* Stream方式追加消息，Redis5.0新增
             * 每个Stream都有一个唯一的名称，这个唯一的名称就是Redis的Key,首次使用时自动创建
             * Stream 消息的key如果是自动创建的，格式则为：毫秒时间戳-序列号(1631607844376-0)表示在1631607844376毫秒内第一条消息
             * 可以支持手动创建  格式必须为整数-整数，而且后面加入的消息id必须大于前面的id
             */
            var result = new RedisValue(null);
            var redis = RedisManager.redisManager.GetDatabase();
            var jsonStr = JsonConvert.SerializeObject(message);
            switch (message.MsgType)
            {
                case MsgType.One: // 单聊
                    result = await redis.StreamAddAsync($"{ONE}", "data", jsonStr, maxLength: MAX_LENGTH_ONE,
                        useApproximateMaxLength: false); // 添加历史记录
                    redis.Publish($"{SUBSCRIBE}", jsonStr); // 推送消息
                    break;
                case MsgType.Group: // 群聊
                    result = await redis.StreamAddAsync($"{GROUP}-{HISTORY}_{message.TargetId}", "data", jsonStr, maxLength: MAX_LENGTH_GROUP, useApproximateMaxLength: false); // 添加历史记录
                    redis.Publish($"{SUBSCRIBEGROUP}", jsonStr);  // 推送消息
                    break;
            }
            return result;
        }
        /// <summary>
        /// 获取历史记录
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="msgTime"></param>
        /// <returns></returns>
        public async Task<List<Message>> GetHistoryMessage(string channel, string msgTime = "0-0")
        {
            var listMsg = new List<Message>();

            var redis = RedisManager.redisManager.GetDatabase();
            var msgList = await redis.StreamReadAsync(channel, msgTime);

            foreach (var streamEntry in msgList)
            {
                foreach (var value in streamEntry.Values)
                {
                    var valueStr = $"{value.Value}";
                    if (string.IsNullOrEmpty(valueStr)) continue;
                    try
                    {
                        var message = JsonConvert.DeserializeObject<Message>(valueStr);
                        if (message is null) continue;
                        listMsg.Add(message);
                    }
                    catch (Exception ex)
                    {
                        await Console.Error.WriteLineAsync($"序列化数据出错：Source={valueStr},Exception={ex.Message}");
                    }
                }
            }

            return listMsg;
        }
    }
}
