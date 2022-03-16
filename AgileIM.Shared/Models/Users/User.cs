using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgileIM.Shared.Models.Users
{
    public class User
    {
        public string Id { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string Nick { get; set; }
        public string Photo { get; set; }
    }
}
