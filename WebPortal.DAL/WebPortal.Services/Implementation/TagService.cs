using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using WebPortal.Application.Dtos;
using WebPortal.Domain;
using WebPortal.Persistence.Infrastructure;

namespace Services.Implementation;

public class TagService : ITagService
{
    private readonly IRepository<Tag> _tagRepository;
    public TagService(IRepository<Tag> tagRepository)
    {
        _tagRepository = tagRepository;
    }

    public async Task AssignTagsToArticle(IEnumerable<TagDto>? tagDtos, Article article)
    {
        if (tagDtos == null)
        {
            return;
        }
        var tags = await _tagRepository.Query()
            .Include(tag => tag.Articles)
            .Where(tag => tagDtos.Select(dto => dto.Name).Contains(tag.Name))
            .ToListAsync();
        var notCreatedTags = tagDtos
            .Select(dto => dto.Name)
            .Except(tags.Select(tag => tag.Name))
            .Select(tagName => new Tag
            {
                Name = tagName,
                Articles = new List<Article>
                {
                    article
                }
            });
        foreach (var tag in tags.Where(tag => !tag.Articles.Select(article => article.Id).Contains(article.Id)))
        {
            tag.Articles.Add(article);
        }
        tags.AddRange(notCreatedTags);
        article.Tags = tags;
        await _tagRepository.AddRangeAsync(notCreatedTags);
        await _tagRepository.SaveChangesAsync();
    }
}