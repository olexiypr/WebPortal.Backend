using Microsoft.AspNetCore.Mvc;

namespace WebPortal.WebAPI.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class ComplaintController : BaseController
{
    [HttpGet("{nickName}")]
    public async Task<ActionResult> GetAllUserComplaints(string nickName)
    {
        return NoContent();
    }

    [HttpGet("{articleId:guid}")]
    public async Task<ActionResult> GetAllArticleComplaints(Guid articleId)
    {
        return NoContent();
    }
}