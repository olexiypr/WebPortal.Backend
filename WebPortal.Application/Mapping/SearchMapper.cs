using AutoMapper;
using WebPortal.Application.Models;
using WebPortal.Domain;

namespace WebPortal.Application.Mapping;

public class SearchMapper : Profile
{
    public SearchMapper()
    {
        CreateMap<IEnumerable<Article>, SearchModel>()
            .ForMember(model => model.Articles,
                opt => opt.MapFrom(articles => articles));
    }
}