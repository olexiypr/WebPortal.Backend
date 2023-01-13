using WebPortal.Application.Dtos;
using WebPortal.Domain;

namespace WebPortal.Services.Interfaces;

public interface ITagService
{
    public Task AssignTagsToArticle(IEnumerable<TagDto>? tagDtos, Article article);
}