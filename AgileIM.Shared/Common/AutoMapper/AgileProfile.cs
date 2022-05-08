using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgileIM.Shared.Models.Users.Dto;
using AgileIM.Shared.Models.Users.Entity;
using AutoMapper;

namespace AgileIM.Shared.Common.AutoMapper
{
    public class AgileProfile:Profile
    {
        public AgileProfile()
        {
            CreateMap<LoginUserDto,UserInfoDto>();
            CreateMap<User,UserInfoDto>();
        }
    }
}
