using WebPortal.Application.Dtos.Commentary;
using WebPortal.Application.Models.Commentary;

namespace Services.Interfaces;

public interface ICommentaryService
{
    public Task<IEnumerable<CommentaryModel>> GetCommentariesByArticleIdAsync(Guid id);
    public Task<CommentaryModel> AddCommentaryToArticleAsync(AddCommentaryDto addCommentaryDto);
    public Task<string> UpdateCommentary(Guid id, string text);
    public Task<bool> DeleteCommentaryById(Guid id);
    public Task<int> AddLikeToCommentary(Guid id);
    public Task<int> AddDislikeToCommentary(Guid id);
}