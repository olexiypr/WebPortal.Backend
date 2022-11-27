using AutoMapper;
using WebPortal.Application.Dtos;
using WebPortal.Application.Dtos.Auth;
using WebPortal.Application.Dtos.User;
using WebPortal.Application.Models;
using WebPortal.Domain.User;

namespace WebPortal.Application.Mapping;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<RegisterUserDto, User>()
            .ForMember(user => user.Avatar, 
                opt => opt.Ignore());
        CreateMap<User, AuthDto>();
        CreateMap<User, UserModel>();
    }
}