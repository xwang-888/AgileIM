using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgileIM.Shared.Models.Users.Request
{
    public class LoginRequest
    {
        /// <summary>
        /// 用户账号
        /// </summary>
        public string UserAccountOrMobile { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord { get; set; }
    }
}
