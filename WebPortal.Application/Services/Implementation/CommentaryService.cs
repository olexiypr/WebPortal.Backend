using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebPortal.Application.Dtos.Commentary;
using WebPortal.Application.Models;
using WebPortal.Application.Models.Commentary;
using WebPortal.Application.Services.Interfaces;
using WebPortal.Domain;
using WebPortal.Persistence.Exceptions;
using WebPortal.Persistence.Infrastructure;

namespace WebPortal.Application.Services.Implementation;

public class CommentaryService : ICommentaryService
{
    private readonly IRepository<Commentary> _commentaryRepository;
    private readonly IRepository<Article> _articleRepository;
    private readonly IMapper _mapper;

    public CommentaryService(IRepository<Commentary> commentaryRepository, IMapper mapper, IRepository<Article> articleRepository)
    {
        _commentaryRepository = commentaryRepository;
        _mapper = mapper;
        _articleRepository = articleRepository;
    }

    public async Task<IEnumerable<CommentaryModel>> GetCommentariesByArticleIdAsync(Guid id) //need to work
    {
        var commentaries = await _commentaryRepository.Query()
            .Include(commentary => commentary.Author)
            .Include(commentary => commentary.Replies)
            .ThenInclude(reply => reply.Replies)
            .Where(commentary => commentary.ArticleId == id)
            .ToListAsync();
        return CommentaryTreeMapper.MapToTree(commentaries);
    }
    
    public async Task<CommentaryModel> AddCommentaryToArticleAsync(AddCommentaryDto addCommentaryDto)
    {
        if (addCommentaryDto.ReplayToId != null)
        {
            var commentary = await _commentaryRepository.Query()
                .Include(commentary => commentary.Replies)
                .FirstOrDefaultAsync(commentary => commentary.Id == addCommentaryDto.ReplayToId);
            if (commentary == null)
            {
                throw new NotFoundException(nameof(Commentary), addCommentaryDto.ReplayToId);
            }

            var addedCommentary = _mapper.Map<Commentary>(addCommentaryDto);
            addedCommentary.Parent = commentary;
            commentary.Replies.Add(addedCommentary);
            await _commentaryRepository.SaveChangesAsync();
            return _mapper.Map<CommentaryModel>(addedCommentary);
        }
        
        var article = await _articleRepository.Query()
            .Include(article => article.Commentaries)
            .Include(article => article.Author)
            .FirstOrDefaultAsync(article => article.Id == addCommentaryDto.ArticleId);
        if (article == null)
        {
            throw new NotFoundException(nameof(Commentary), addCommentaryDto.ArticleId);
        }

        var comment = _mapper.Map<Commentary>(addCommentaryDto);
        article.Commentaries.Add(comment);
        await _articleRepository.SaveChangesAsync();
        var commentaryModel = _mapper.Map<CommentaryModel>(comment);
        commentaryModel.AuthorNickName = article.Author.NickName;
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