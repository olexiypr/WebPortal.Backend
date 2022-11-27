using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WebPortal.Application.Dtos.User;
using WebPortal.Application.Exceptions;
using WebPortal.Application.Models;
using WebPortal.Application.Models.Article;
using WebPortal.Application.Services.Interfaces;
using WebPortal.Domain;
using WebPortal.Domain.User;
using WebPortal.Persistence.Infrastructure;

namespace WebPortal.Application.Services.Implementation;

public class RecommendationService : IRecommendationService
{
    private readonly IArticleService _articleService;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Article> _articleRepository;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IMapper _mapper;

    public RecommendationService(IArticleService articleService, IMapper mapper, IRepository<Article> articleRepository, IHttpContextAccessor contextAccessor, IRepository<User> userRepository)
    {
        _contextAccessor = contextAccessor;
        _userRepository = userRepository;
        (_articleService, _mapper, _articleRepository) = (articleService, mapper, articleRepository);
    }

    public async Task<IEnumerable<ArticlePreviewModel>> GetRecommendation() //need to work
    {
        var userId = _contextAccessor.HttpContext!.User.GetCurrentUserId();
        var user = await _userRepository.Query()
            .Include(user => user.Recommendation)
            .FirstOrDefaultAsync(user => user.Id == userId);
        if (user == null)
        {
            
        }

        /*user.Recommendation.FoundWords;*/
        var recommendationModel = await _mapper.ProjectTo<ArticlePreviewModel>(_articleRepository.Query().AsQueryable())
            .ToListAsync();
        return recommendationModel;
    }

    public Task<IEnumerable<ArticlePreviewModel>> GetPopularArticles()
    {
        return null;
    }
}