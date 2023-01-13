using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebPortal.Application.Dtos;
using WebPortal.Application.Dtos.ArticleCategory;
using WebPortal.Services.Interfaces;

namespace WebPortal.WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ArticleCategoryController : BaseController
{
    private readonly IArticleCategoryService _articleCategoryService;

    public ArticleCategoryController(IArticleCategoryService articleCategoryService) =>
        (_articleCategoryService) = (articleCategoryService);

    [HttpGet]
    public async Task<ActionResult> GetAllCategories()
    {
        var categories = await _articleCategoryService.GetAllCategoriesAsync();
        return Ok(categories);
    }
    
    [HttpGet("{categoryId:guid}")]
    public async Task<ActionResult> GetArticlesInCategory(Guid categoryId, [FromQuery] PaginationDto paginationDto)
    {
        var articleCategoryModel = await _articleCategoryService.GetArticlesInCategory(categoryId, paginationDto);
        return Ok(articleCategoryModel);
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult> AddCategory(CreateArticleCategoryDto createArticleCategoryDto)
    {
        var category = await _articleCategoryService.CreateArticleCategory(createArticleCategoryDto);
        return Ok(category);
    } 
}