using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgileIM.Shared.Models.Users.Request
{
    public class FriendRequest
    {
        public string UserId { get; set; }
        public string FriendId { get; set; }
    }
}
