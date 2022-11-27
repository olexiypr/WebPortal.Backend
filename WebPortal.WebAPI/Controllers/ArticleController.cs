using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebPortal.Application.Dtos;
using WebPortal.Application.Dtos.Article;
using WebPortal.Application.Dtos.Complain;
using WebPortal.Application.Models.Article;
using WebPortal.Application.Services.Interfaces;

namespace WebPortal.WebAPI.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class ArticleController : BaseController
{
    private readonly IArticleService _articleService;
    public ArticleController(IArticleService articleService) 
        => (_articleService) = (articleService);
    
    [HttpGet("{id:Guid}")]
    public async Task<ActionResult> GetArticleById(Guid id)
    {
        var article = await _articleService.GetArticleByIdAsync(id);
        return Ok(article);
    }

    [HttpGet]
    [ActionName("popular")]
    public async Task<ActionResult<IEnumerable<ArticlePreviewModel>>> GetPopularArticles([FromQuery] string period)
    {
        var articles = await _articleService.GetPopularArticles(period);
        return Ok(articles);
    }
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> CreateArticle([FromBody]CreateArticleDto createArticleDto)
    {
        var articleModel = await _articleService.CreateArticleAsync(createArticleDto);
        return Ok(articleModel);
    }
    
    [HttpPut]
    [Authorize]
    [ActionName("data")]
    public async Task<ActionResult> UpdateArticle(UpdateArticleDataDto updateArticleDataDto)
    {
        var articleModel = await _articleService.UpdateArticleDataAsync(updateArticleDataDto);
        return Ok(articleModel);
    }

    [HttpPut]
    [Authorize]
    [ActionName("analytics")]
    public async Task<ActionResult> UpdateArticleAnalytics(UpdateArticleAnalyticsDto updateArticleAnalyticsDto)
    {
        var result = await _articleService.UpdateArticleAnalyticsAsync(updateArticleAnalyticsDto);
        return Ok(new
        {
            countLikes = result.Item1,
            reting = result.Item2
        });
    }
    [HttpDelete("{id:Guid}")]
    [Authorize]
    public async Task<ActionResult> DeleteArticle(Guid id)
    {
        var isSuccess = await _articleService.DeleteArticle(id);
        return Ok(isSuccess);
    }
    /*[HttpPut]
    public async Task<ActionResult> Complain(ArticleComplainDto articleComplainDto)
    {
        return NoContent();
    }*/
}