using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using WebPortal.Application.Dtos.Auth;
using WebPortal.Application.Dtos.User;
using WebPortal.Application.Services.Interfaces;

namespace WebPortal.WebAPI.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class AuthController : BaseController
{
    private readonly IRegisterService _registerService;
    private readonly IMapper _mapper;
    public AuthController(IRegisterService registerService, IMapper mapper) => (_registerService, _mapper) = (registerService, mapper);
    
    [HttpPost]
    [ActionName("login")]
    public async Task<IActionResult> Login(AuthDto authDto)
    {
        var (token, user) = await _registerService.LoginUserAsync(authDto);
        var response = new
        {
            access_token = token,
            user = user
        };
        return Ok(response);
    }
    [HttpPost]
    [ActionName("register")]
    public async Task<ActionResult> Register([FromForm]RegisterUserDto registerUserDto)
    {
        var (token, user) = await _registerService.RegisterUserAsync(registerUserDto);
        var response = new
        {
            access_token = token,
            user = user
        };
        return Ok(response);
    }
}