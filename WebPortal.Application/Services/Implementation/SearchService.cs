using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WebPortal.Application.Exceptions;
using WebPortal.Application.Models;
using WebPortal.Application.Services.Interfaces;
using WebPortal.Domain;
using WebPortal.Domain.User;
using WebPortal.Persistence.Infrastructure;

namespace WebPortal.Application.Services.Implementation;

public class SearchService : ISearchService
{
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Article> _articleRepository;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IMapper _mapper;

    public SearchService(IRepository<User> userRepository, IRepository<Article> articleRepository, IMapper mapper, IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
        (_userRepository, _articleRepository, _mapper) = (userRepository, articleRepository, mapper);
    }

    public async Task<SearchModel> Search(string searchText)
    {
        var id = _contextAccessor.HttpContext!.User.GetCurrentUserId();
        var user = await _userRepository.Query()
            .Include(user => user.Recommendation)
            .FirstOrDefaultAsync(user => user.Id == id);
        if (user == null)
        {
            
        }
        user.Recommendation ??= new Recommendation();
        user.Recommendation.FoundWords.AddRange(searchText.Split(" "));
        var articles = _articleRepository.Query().Select(a => a).ToList().Where(article =>
        {
            foreach (var s in searchText.Split(' '))
            {
                if (article.KeyWords.Contains(s))
                {
                    return true;
                }
            }
            return false;
        });
        return _mapper.Map<SearchModel>(articles);
    }
}