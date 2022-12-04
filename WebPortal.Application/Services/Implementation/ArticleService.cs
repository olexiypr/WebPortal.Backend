using System.Diagnostics;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Npgsql.PostgresTypes;
using WebPortal.Application.Dtos;
using WebPortal.Application.Dtos.Article;
using WebPortal.Application.Exceptions;
using WebPortal.Application.Models;
using WebPortal.Application.Models.Article;
using WebPortal.Application.Services.Interfaces;
using WebPortal.Domain;
using WebPortal.Domain.Enums;
using WebPortal.Domain.User;
using WebPortal.Persistence.Exceptions;
using WebPortal.Persistence.Infrastructure;

namespace WebPortal.Application.Services.Implementation;

public class ArticleService : IArticleService
{
    private readonly IRepository<Article> _articleRepository;
    private readonly IRepository<User> _userRepository;
    /*private readonly IRepository<Tag> _tagRepository;*/
    private readonly IRepository<ArticleCategory> _categoryRepository;
    private readonly ITagService _tagService;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IMapper _mapper;

    public ArticleService(IRepository<Article> articleRepository, IRepository<User> userRepository, IRepository<ArticleCategory> categoryRepository,IMapper mapper, IHttpContextAccessor contextAccessor, ITagService tagService)
    {
        _contextAccessor = contextAccessor;
        _tagService = tagService;
        (_articleRepository, _userRepository, _categoryRepository, _mapper) = (articleRepository,
            userRepository, categoryRepository, mapper);
    }

    public async Task<ArticleModel> GetArticleByIdAsync(Guid id)
    {
        var article = await _articleRepository.Query()
            .Include(article => article.Author)
            .Include(article => article.Category)
            .Include(article => article.Tags)
            .FirstOrDefaultAsync(article => article.Id == id);
        if (article == null)
        {
            throw new NotFoundException(nameof(Article), id);
        }
        ViewArticle(article);
        await _articleRepository.SaveChangesAsync();
        return _mapper.Map<ArticleModel>(article);
    }

    public async Task<IEnumerable<UserArticlePreviewModel>> GetUserArticles(ArticleStatuses status)
    {
        var userId = _contextAccessor.HttpContext!.User.GetCurrentUserId();
        var articles = await _articleRepository.Query()
            .Where(article => article.Status == status && article.AuthorId == userId)
            .ToListAsync();
        var articlePreview = _mapper
            .ProjectTo<UserArticlePreviewModel>(articles.AsQueryable())
            .ToList();
        return articlePreview;
    }

    private void ViewArticle(Article article)
    {
        article.CountViews++;
        article.CountViewsPerDay++;
        article.CountViewsPerWeek++;
        article.CountViewsPerMonth++;
    }
    public async Task<IEnumerable<ArticlePreviewModel>> GetPopularArticles(string period)
    {
        var articles = period switch
        {
            "day" => await _articleRepository.Query()
                .OrderByDescending(article => article.CountViewsPerDay)
                .Take(10)
                .ToListAsync(),
            "week" => await _articleRepository.Query()
                .OrderByDescending(article => article.CountViewsPerWeek)
                .Take(10)
                .ToListAsync(),
            "month" => await _articleRepository.Query()
                .OrderByDescending(article => article.CountViewsPerMonth)
                .Take(10)
                .ToListAsync(),
            _ => throw new ArgumentException(nameof(period), period)
        };
        return _mapper.ProjectTo<ArticlePreviewModel>(articles.AsQueryable());
    }
    

    public async Task<ArticleModel> CreateArticleAsync(CreateArticleDto articleDto)
    {
        var article = _mapper.Map<Article>(articleDto);
        var category = await _categoryRepository.Query()
            .FirstOrDefaultAsync(articleCategory => articleCategory.Id == articleDto.CategoryId);
        if (category == null)
        {
            throw new ArgumentException();
        }
        var userId = _contextAccessor.HttpContext!.User.GetCurrentUserId();
        article.KeyWords = GetKeyWords(article.Text, article.Name);
        await _tagService.AssignTagsToArticle(articleDto!.Tags, article);
        await _articleRepository.AddAsync(article);
        await _articleRepository.SaveChangesAsync();
        var articleModel = _mapper.Map<ArticleModel>(article);
        articleModel.AuthorNickName = (await _userRepository.GetByIdAsync(userId)).NickName;
        return articleModel;
    }

    public async Task<ArticleModel> UpdateArticleDataAsync(UpdateArticleDataDto updateArticleDataDto)
    {
        var article = await _articleRepository.Query().
            Include(article => article.Author)
            .Include(article => article.Category)
            .FirstOrDefaultAsync(article => article.Id == updateArticleDataDto.Id);
        if (article == null)
        {
            throw new NotFoundException(nameof(article), updateArticleDataDto.Id);
        }
        var userId = _contextAccessor.HttpContext!.User.GetCurrentUserId();
        if (article.AuthorId != userId)
        {
            throw new UserAccessDeniedExceptions(nameof(UpdateArticleDataDto));
        }
        article.Name = updateArticleDataDto.Name ?? article.Name;
        if (updateArticleDataDto.Text != null)
        {
            article.Text = updateArticleDataDto.Text;
            article.KeyWords = GetKeyWords(updateArticleDataDto.Text, article.Name);
        }
        if (updateArticleDataDto.Tags != null)
        {
            await _tagService.AssignTagsToArticle(updateArticleDataDto.Tags, article);
        }
        await _articleRepository.SaveChangesAsync();
        var articleModel = _mapper.Map<ArticleModel>(article);
        articleModel.Tags = _mapper.ProjectTo<TagModel>(article.Tags.AsQueryable());
        return articleModel;
    }

    public async Task<(int, double)> UpdateArticleAnalyticsAsync(UpdateArticleAnalyticsDto updateArticleAnalyticsDto)
    {
        var article = await _articleRepository.GetByIdAsync(updateArticleAnalyticsDto.Id);
        if (updateArticleAnalyticsDto.Rating != null)
        {
            article.Rating = 
                ((double)(article.Rating * article.CountAppraisers + updateArticleAnalyticsDto.Rating) / 
                 (double)(article.CountAppraisers + 1));
            article.CountAppraisers++;
        }
        
        if (updateArticleAnalyticsDto.IsLike)
        {
            article.CountLikes++;
        }

        await _articleRepository.SaveChangesAsync();
        await _userRepository.SaveChangesAsync();
        return (article.CountLikes, article.Rating);
    }

    public async Task<bool> DeleteArticle(Guid id)
    {
        var article = await _articleRepository.GetByIdAsync(id);
        _articleRepository.Delete(article);
        await _articleRepository.SaveChangesAsync();
        return true;
    }

    private List<string> GetKeyWords(string text, string articleName)
    {
        var keyWords = new List<string>(articleName.Split(" "));
        var textArr = text.Split(' ')
            .GroupBy(word => word)
            .Select(word => new {Word = word.Key, Count = word.Count()})
            .OrderByDescending(g => g.Count);
        var count = textArr.First().Count / 2;
        for (int i = 0; i < count; i++)
        {
            keyWords.Add(textArr.Select(g => g.Word).ToArray()[i]);
        }
        return keyWords;
    }
}