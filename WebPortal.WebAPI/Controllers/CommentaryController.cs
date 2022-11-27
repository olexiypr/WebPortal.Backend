using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebPortal.Application.Dtos.Commentary;
using WebPortal.Application.Services.Interfaces;

namespace WebPortal.WebAPI.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class CommentaryController : BaseController
{
    private readonly ICommentaryService _commentaryService;

    public CommentaryController(ICommentaryService commentaryService)
        => (_commentaryService) = (commentaryService);
    
    [HttpGet("{articleId:Guid}")]
    public async Task<ActionResult> GetCommentariesByArticleId(Guid articleId)
    {
        var commentaryModel = await _commentaryService.GetCommentariesByArticleIdAsync(articleId);
        return Ok(commentaryModel);
    }
    
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> AddCommentaryToArticle([FromBody]AddCommentaryDto addCommentaryDto)
    {
        var commentary = await _commentaryService.AddCommentaryToArticleAsync(addCommentaryDto);
        return Ok(commentary);
    }
    
    [HttpPut("{commentaryId:Guid}")]
    [Authorize]
    public async Task<ActionResult> UpdateCommentary(Guid commentaryId, [FromBody] string text)
    {
        var commentaryText = await _commentaryService.UpdateCommentary(commentaryId, text);
        return Ok(commentaryText);
    }
    
    [HttpDelete("{commentaryId:Guid}")]
    [Authorize]
    public async Task<ActionResult> DeleteCommentary(Guid commentaryId)
    {
        var isSuccess = await _commentaryService.DeleteCommentaryById(commentaryId);
        return Ok(isSuccess);
    }
    
    [ActionName("add_like")]
    [HttpPut("{commentaryId:Guid}")]
    [Authorize]
    public async Task<ActionResult> AddLikeToComment(Guid commentaryId)
    {
        var countLikes = await _commentaryService.AddLikeToCommentary(commentaryId); 
        return Ok(countLikes);
    }
    
    [ActionName("add_dislike")]
    [HttpPut("{commentaryId:Guid}")]
    [Authorize]
    public async Task<ActionResult> AddDislikeToComment(Guid commentaryId)
    {
        var countDislikes = await _commentaryService.AddDislikeToCommentary(commentaryId); 
        return Ok(countDislikes);
    }
}