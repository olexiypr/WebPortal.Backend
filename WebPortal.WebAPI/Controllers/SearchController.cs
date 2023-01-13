using Microsoft.AspNetCore.Mvc;
using WebPortal.Services.Interfaces;

namespace WebPortal.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SearchController : BaseController
{
    private readonly ISearchService _searchService;

    public SearchController(ISearchService searchService) =>
        (_searchService) = (searchService);
    
    [HttpPost]
    public async Task<ActionResult> Search([FromBody] string searchText)
    {
        var searchModel = await _searchService.Search(searchText);
        return Ok(searchModel);
    }
}