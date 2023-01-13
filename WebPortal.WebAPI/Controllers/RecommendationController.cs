using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using WebPortal.Application.Models;
using WebPortal.Application.Models.Article;

namespace WebPortal.WebAPI.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class RecommendationController : BaseController
{
    private readonly IRecommendationService _recommendationService;

    public RecommendationController(IRecommendationService recommendationService) =>
        (_recommendationService) = (recommendationService);

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<ArticlePreviewModel>>> GetRecommendation()
    {
        var recommendation = await _recommendationService.GetRecommendation();
        return Ok(recommendation);
    }
}