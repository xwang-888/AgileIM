using AgileIM.Shared.Models.Im;

using Newtonsoft.Json;

using StackExchange.Redis;

using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace AgileIM.Service.Helper
{
    public class ImServer
    {
        public ImServer()
        {
            Initialization();
            _clients = new ConcurrentDictionary<Guid, ConcurrentDictionary<Guid, ImServerClient>>();
        }

        private readonly ConcurrentDictionary<Guid, ConcurrentDictionary<Guid, ImServerClient>> _clients;
        /// <summary>
        /// 初始化数据
        /// </summary>
        public void Initialization()
        {
            // 订阅消息
            ImHelper.Default.Subscribe(ImHelper.SUBSCRIBE, RedisSubscribeMessage);
            ImHelper.Default.Subscribe(ImHelper.SUBSCRIBEGROUP, RedisSubscribeGroupMessage);

            var redis = RedisManager.redisManager.GetDatabase();
            #region Create Channel
            // 判断此键是否存在，不存在则创建
            if (!redis.HashExists(ImHelper.SUBSCRIBE, ""))
                redis.HashSet(ImHelper.SUBSCRIBE, "", "");
            if (!redis.HashExists(ImHelper.SUBSCRIBEGROUP, ""))
                redis.HashSet(ImHelper.SUBSCRIBEGROUP, "", "");
            #endregion
        }
        /// <summary>
        /// 拦截器，拦截websocket连接
        /// </summary>
        /// <param name="content"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task Acceptor(HttpContext content, Func<Task> next)
        {
            if (content.WebSockets.IsWebSocketRequest)
            {
                var isOk = TokenHelper.GetAllClaim(content, out var tokenInfo);
                if (!isOk) return;
                if (string.IsNullOrEmpty(tokenInfo.UserId)) return;

                var socket = await content.WebSockets.AcceptWebSocketAsync();

                var clientId = Guid.Parse(tokenInfo.UserId);
                var imServerClient = new ImServerClient(socket, clientId);
                var socketList = _clients.GetOrAdd(clientId, clintId => new ConcurrentDictionary<Guid, ImServerClient>());
                var newId = Guid.NewGuid();
                socketList.TryAdd(newId, imServerClient);

                var buffer = new byte[4096];
                try
                {
                    while (socket.State == WebSocketState.Open && _clients.ContainsKey(clientId))
                    {
                        var wsReceive = await socket.ReceiveAsync(buffer, default);
                        var str = System.Text.Encoding.UTF8.GetString(buffer);
                        var msg = JsonConvert.DeserializeObject<Message>(str);
                        if (msg is null) continue;

                        if (msg.MsgType == MsgType.Heartbeat)
                        {
                            await socket.SendAsync(buffer, WebSocketMessageType.Text,
                                true, CancellationToken.None);
                        }
                        var outgoing = new ArraySegment<byte>(buffer, 0, wsReceive.Count);
                    }
                    socket.Abort();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                socketList.TryRemove(newId, out var oldClient);

                if (!socketList.Any())
                    _clients.TryRemove(clientId, out var oldList);
            }
            else
            {
                await next();
            }
        }
        /// <summary>
        /// 订阅单聊消息
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="msg"></param>
        private void RedisSubscribeMessage(RedisChannel channel, RedisValue msg)
        {
            try
            {
                var message = JsonConvert.DeserializeObject<Message>(msg.ToString());
                if (message is null) return;
                if (string.IsNullOrEmpty(message.FromId)) return;

                //SendMessage(message.FromId, msg); // 消息推送给发送者，多端消息同步
                if (string.IsNullOrEmpty(message.FromId) || message.FromId.Equals(message.TargetId)) // 判断接收者是否为发送者,接收者为发送者不发送
                    return;

                // 消息推送到接收者
                if (_clients.TryGetValue(Guid.Parse(message.TargetId), out var clients))
                {
                    foreach (var imServerClient in clients)
                    {
                        // 判断是否为发送者
                        if ($"{imServerClient.Value.ClientId}".Equals(message.FromId)) continue;

                        imServerClient.Value.WebSocket.SendAsync(new ArraySegment<byte>(msg), WebSocketMessageType.Text,
                            true, CancellationToken.None);
                        //记录日志
                        Console.WriteLine($"{message.FromId}发送了消息给{message.TargetId}，消息内容为：{message.Content}");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}单聊");
            }
        }
        /// <summary>
        /// 订阅群聊消息
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="msg"></param>
        private void RedisSubscribeGroupMessage(RedisChannel channel, RedisValue msg)
        {
            try
            {
                var message = JsonConvert.DeserializeObject<Message>(msg.ToString());
                if (message is null) return;
                // 在redis中拿到群聊信息
                var redis = RedisManager.redisManager.GetDatabase();
                var groupList = redis.HashGetAll($"{ImHelper.GROUP}_{message.TargetId}");
                if (groupList.Length <= 0) return;

                foreach (var guid in groupList.Select(a => a.Name))
                {
                    //SendMessage(guid.ToString(), msg, MsgType.Group);  // 推送消息给群聊用户,多端消息同步
                    #region OldCode
                    if (_clients.TryGetValue(Guid.Parse(guid.ToString()), out var clients))
                    {
                        foreach (var imServerClient in clients)
                        {
                            // 判断是否为发送者
                            if ($"{imServerClient.Value.ClientId}".Equals(message.FromId)) continue;

                            imServerClient.Value.WebSocket.SendAsync(new ArraySegment<byte>(msg), WebSocketMessageType.Text,
                                        true, CancellationToken.None);
                            // 记录日志
                            Console.WriteLine($"{message.FromId}发送了群聊消息到{message.TargetId},消息内容为：{message.Content}");
                        }
                    }
                    #endregion
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}群聊");
            }
        }
    }
    public class ImServerClient
    {
        public ImServerClient(WebSocket webSocket, Guid clientId)
        {
            ClientId = clientId;
            WebSocket = webSocket;
        }
        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid ClientId { get; set; }
        /// <summary>
        /// 用户的WebSocket
        /// </summary>
        public WebSocket WebSocket { get; set; }
    }

    public static class ImServerExtension
    {
        /// <summary>
        /// websocket的请求地址
        /// </summary>
        public const string PathMatch = "/ws";
        private static bool _isUseWebSockets = false;
        public static IApplicationBuilder UseImServer(this IApplicationBuilder app)
        {
            app.Map(PathMatch, appCut =>
            {
                var imServer = new ImServer();
                if (!_isUseWebSockets)
                {
                    _isUseWebSockets = true;
                    app.UseWebSockets();
                }
                appCut.Use(imServer.Acceptor);
            });

            return app;
        }
    }
}
