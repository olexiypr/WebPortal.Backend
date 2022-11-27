using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace WebPortal.WebAPI.Controllers;

public class BaseController : ControllerBase
{
    internal Guid UserId => !User.Identity.IsAuthenticated ? Guid.Empty : 
        Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
}