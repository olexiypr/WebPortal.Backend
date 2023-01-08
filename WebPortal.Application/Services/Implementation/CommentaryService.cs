using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using WebPortal.Application.Dtos.Commentary;
using WebPortal.Application.Extensions;
using WebPortal.Application.Models;
using WebPortal.Application.Models.Commentary;
using WebPortal.Application.Services.Interfaces;
using WebPortal.Domain;
using WebPortal.Domain.User;
using WebPortal.Persistence.Exceptions;
using WebPortal.Persistence.Infrastructure;

namespace WebPortal.Application.Services.Implementation;

public class CommentaryService : ICommentaryService
{
    private readonly IRepository<Commentary> _commentaryRepository;
    private readonly IRepository<Article> _articleRepository;
    private readonly IMapper _mapper;
    private readonly IRepository<User> _userRepository;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IMemoryCache _memoryCache;

    public CommentaryService(IRepository<Commentary> commentaryRepository, IMapper mapper, IRepository<Article> articleRepository, IHttpContextAccessor contextAccessor, IRepository<User> userRepository, IMemoryCache memoryCache)
    {
        _commentaryRepository = commentaryRepository;
        _mapper = mapper;
        _articleRepository = articleRepository;
        _contextAccessor = contextAccessor;
        _userRepository = userRepository;
        _memoryCache = memoryCache;
    }

    public async Task<IEnumerable<CommentaryModel>> GetCommentariesByArticleIdAsync(Guid id) //need to work
    {
        if (_memoryCache.TryGetValue(id, out IEnumerable<Commentary> commentaries))
            return CommentaryTreeMapper.MapToTree(commentaries);
        commentaries = await _commentaryRepository.Query()
            .Include(commentary => commentary.Author)
            .Include(commentary => commentary.Replies)
            .ThenInclude(reply => reply.Replies)
            .Where(commentary => commentary.ArticleId == id)
            .ToListAsync();
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(20));
        _memoryCache.Set(id, commentaries, cacheEntryOptions);
        return CommentaryTreeMapper.MapToTree(commentaries);
    }
    
    public async Task<CommentaryModel> AddCommentaryToArticleAsync(AddCommentaryDto addCommentaryDto)
    {
        var addedCommentary = _mapper.Map<Commentary>(addCommentaryDto);
        var article = await _articleRepository.Query()
            .Include(article => article.Commentaries)
            .FirstOrDefaultAsync(article => article.Id == addCommentaryDto.ArticleId);
        if (article == null)
        {
            throw new NotFoundException(nameof(Commentary), addCommentaryDto.ArticleId);
        }
        addedCommentary.Article = article;
        var authorId = _contextAccessor.HttpContext!.User.GetCurrentUserId();
        addedCommentary.Author = await _userRepository.GetByIdAsync(authorId);
        if (addCommentaryDto.ReplayToId != null)
        {
            var commentary = await _commentaryRepository.Query()
                .Include(commentary => commentary.Replies)
                .FirstOrDefaultAsync(commentary => commentary.Id == addCommentaryDto.ReplayToId);
            if (commentary == null)
            {
                throw new NotFoundException(nameof(Commentary), addCommentaryDto.ReplayToId);
            }
            addedCommentary.Parent = commentary;
            commentary.Replies.Add(addedCommentary);
        }
        await _commentaryRepository.AddAsync(addedCommentary);
        await _commentaryRepository.SaveChangesAsync();
        var commentaryModel = _mapper.Map<CommentaryModel>(addedCommentary);
        return commentaryModel;
    }

    public async Task<string> UpdateCommentary(Guid id, string text)
    {
        var commentary = await _commentaryRepository.GetByIdAsync(id);
        commentary.Text = text;
        await _commentaryRepository.SaveChangesAsync();
        return commentary.Text;
    }

    public async Task<bool> DeleteCommentaryById(Guid id)
    {
        var commentary = await _commentaryRepository.Query()
            .Include(commentary => commentary.Replies)
            .FirstOrDefaultAsync(commentary => commentary.Id == id);
        if (commentary == null)
        {
            throw new NotFoundException(nameof(Commentary), id);
        }
        _commentaryRepository.Delete(commentary);
        await _commentaryRepository.SaveChangesAsync();
        return true;
    }

    public async Task<int> AddLikeToCommentary(Guid id)
    {
        var commentary = await _commentaryRepository.GetByIdAsync(id);
        commentary.CountLikes++;
        await _commentaryRepository.SaveChangesAsync();
        return commentary.CountLikes;
    }

    public async Task<int> AddDislikeToCommentary(Guid id)
    {
        var commentary = await _commentaryRepository.GetByIdAsync(id);
        commentary.CountDislikes++;
        await _commentaryRepository.SaveChangesAsync();
        return commentary.CountDislikes;
    }
}