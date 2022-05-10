using System.Security.Claims;

using AgileIM.Service.OAuth.Configs;
using AgileIM.Service.Services;
using AgileIM.Service.Services.Ide4Service;
using AgileIM.Shared.Models.Users;
using AgileIM.Shared.Models.Users.Entity;

using IdentityModel;

using IdentityServer4.Models;
using IdentityServer4.Validation;

namespace AgileIM.Service.OAuth
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {

        public ResourceOwnerPasswordValidator(IConfiguration configuration, IVerifyService verifyService)
        {
            _verifyService = verifyService;
            _configuration = configuration;
        }

        private readonly IVerifyService _verifyService;
        private readonly IConfiguration _configuration;
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = await _verifyService.VerifyUser(context.UserName, context.Password);
            if (user is not null)
            {
                // identity提供者,
                var identityProvider = _configuration["ServerIpPort"] ?? "local";
                // 自定义响应
                var customResponseDic = new Dictionary<string, object> { { "user", user } };
                context.Result = new GrantValidationResult
                    (context.UserName,
                    OidcConstants.AuthenticationMethods.Password,
                    GetClaims(user),
                    identityProvider,
                    customResponseDic);
            }
            else
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "invalid");
            }
        }

        public Claim[] GetClaims(User user)
        {
            return new Claim[]
            {
                new ("tokenExpireTime",DateTime.Now.AddSeconds(Convert.ToInt32(OAuthConfig.ExpireIn)).ToString("yyyy-HH-MM-dd HH:mm:ss"))
            };
        }
    }
}
