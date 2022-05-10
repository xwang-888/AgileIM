using IdentityServer4;
using IdentityServer4.Models;

namespace AgileIM.Service.OAuth.Configs
{
    /// <summary>
    /// IdentityServer4 配置
    /// </summary>
    public class Ide4Config
    {
        /*在3.1.x 到 4.x 的变更中，
         ApiResource 的 Scope 正式独立出来为 ApiScope 对象，
        区别ApiResource 和 Scope的关系, 
        Scope 是属于ApiResource 的一个属性，可以包含多个Scope。*/

        /// <summary>
        /// 获取管理的API Resources
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApiResource => new List<ApiResource>()
        {
            new ("ImService","Im")
            {
                Scopes = { "ImService" }
            }
        };
        /// <summary>
        /// 添加对OpenID Connect的支持
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IdentityResource> GetIdentityResources => new List<IdentityResource>()
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };
        /// <summary>
        /// Authorization Server保护了哪些 API Scope（作用域）
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiScope> GetApiScopes => new[] { new ApiScope("ImService") };
        /// <summary>
        /// 哪些客户端 Client（应用） 可以使用这个 Authorization Server
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IdentityServer4.Models.Client> GetApiClients =>
            new[]
            {
                // 客户端模式
                new IdentityServer4.Models.Client
                {
                    // 客户端唯一标识
                    ClientId = OAuthConfig.ClientUserApi.ClientId,
                    // 名称
                    ClientName = OAuthConfig.ClientUserApi.ClientName,
                    // 进行加密的客户端密码 
                    ClientSecrets = new []{new Secret(OAuthConfig.ClientUserApi.Secret.Sha256())},
                    // 授权方式，客户端认证
                    //授权方式，这里采用的是客户端认证模式，只要ClientId，以及ClientSecrets正确即可访问对应的AllowedScopes里面的api资源
                    AllowedGrantTypes =  GrantTypes.ClientCredentials,
                    // 设置token过期时间为12小时
                    AccessTokenLifetime = OAuthConfig.AccessTokenLifetime,
                    // 允许访问的范围,定义这个客户端可以访问的APi资源数组
                    AllowedScopes = new []
                    {
                        "ImService",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    },
                    // Token所携带的信息
                    Claims = new List<ClientClaim>()
                    {
                        new(IdentityModel.JwtClaimTypes.Role,"admin"),
                        new(IdentityModel.JwtClaimTypes.NickName,"xwang"),
                        new("email","wx1990929@163.com"),
                        new("test","你可以自定义携带的信息"),
                    },
                    // 是否允许脱机访问
                    AllowOfflineAccess = true
                },
                // 密码模式
                new IdentityServer4.Models.Client
                {
                    // 客户端唯一标识
                    ClientId = OAuthConfig.SecretUserApi.ClientId,
                    // 名称
                    ClientName = OAuthConfig.SecretUserApi.ClientName,
                    // 进行加密的客户端密码 
                    ClientSecrets = new [] { new Secret(OAuthConfig.SecretUserApi.Secret.Sha256()) },
                    /*授权方式，这里采用的是密码认证模式，
                     需要client_id，client_secret，grant_type，username，password全部正确
                    才能访问对应的AllowedScopes里面的api资源*/
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    // 设置token过期时间为12小时
                    AccessTokenLifetime = OAuthConfig.AccessTokenLifetime,
                    //允许访问api的范围
                    AllowedScopes = new [] {"ImService",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile },
                    AllowOfflineAccess = true
                },
            };

    }
}
