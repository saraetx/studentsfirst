using AutoMapper;
using StudentsFirst.Common.Dtos.Groups;
using StudentsFirst.Common.Dtos.Users;
using StudentsFirst.Common.Models;

namespace StudentsFirst.Api.Monolithic.Infrastructure.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserResponse>();
            CreateMap<Group, GroupResponse>();
        }
    }
}
