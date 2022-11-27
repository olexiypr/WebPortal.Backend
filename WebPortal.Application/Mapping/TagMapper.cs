using AutoMapper;
using WebPortal.Application.Models;
using WebPortal.Domain;

namespace WebPortal.Application.Mapping;

public class TagMapper : Profile
{
    public TagMapper()
    {
        CreateMap<Tag, TagModel>().ReverseMap();
    }
}