using AgileIM.Shared.Models.ClientModels.Message.Entity;
using AgileIM.Shared.Models.Users.Dto;
using AgileIM.Shared.Models.Users.Entity;

using AutoMapper;

namespace AgileIM.Shared.Common.AutoMapper
{
    public class AgileProfile : Profile
    {
        public AgileProfile()
        {
            CreateMap<LoginUserDto, UserInfoDto>();
            CreateMap<User, UserInfoDto>();
            CreateMap<Messages, MessageDto>();
        }
    }
}
