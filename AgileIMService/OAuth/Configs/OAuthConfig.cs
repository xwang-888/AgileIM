namespace AgileIM.Service.OAuth.Configs
{
    /// <summary>
    /// 鉴权配置信息
    /// </summary>
    public class OAuthConfig
    {
        /// <summary>
        /// Token生命周期
        /// </summary>
        public static int AccessTokenLifetime = ExpireIn;
        /// <summary>
        /// 过期秒数
        /// </summary>
        public const int ExpireIn = 3600 * 12;

        /// <summary>
        /// 客户端模式用户api
        /// </summary>
        public class ClientUserApi
        {
            /// <summary>
            /// 客户端唯一标识
            /// </summary>
            public static string ClientId = "user_clientId";
            /// <summary>
            /// 名称
            /// </summary>
            public static string ClientName = "user_api";
            /// <summary>
            /// 进行加密的客户端密码
            /// </summary>
            public static string Secret = "user_secret";
        }

        /// <summary>
        /// 密码模式用户api
        /// </summary>
        public class SecretUserApi
        {
            /// <summary>
            /// 客户端唯一标识
            /// </summary>
            public static string ClientId = "user_managerId";
            /// <summary>
            /// 名称
            /// </summary>
            public static string ClientName = "user_manager";
            /// <summary>
            /// 进行加密的客户端密码
            /// </summary>
            public static string Secret = "user_managerSecret";
        }

    }
}
