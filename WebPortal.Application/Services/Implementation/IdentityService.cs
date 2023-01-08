using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.RegularExpressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebPortal.Application.Auth;
using WebPortal.Application.Dtos.Auth;
using WebPortal.Application.Exceptions;
using WebPortal.Application.Services.Interfaces;
using WebPortal.Domain.User;
using WebPortal.Persistence.Exceptions;
using WebPortal.Persistence.Infrastructure;

namespace WebPortal.Application.Services.Implementation;

public class IdentityService : IIdentityService
{
    private readonly IRepository<UserAuth> _userRepository;

    public IdentityService(IRepository<UserAuth> userRepository) =>
        (_userRepository) = (userRepository);
    
    public async Task<string> GetToken(AuthDto authDto)
    {
        var user = await _userRepository.Query().FirstOrDefaultAsync(auth => auth.Email == authDto.Email);
        if (user == null)
        {
            throw new NotFoundException(nameof(User), authDto);
        }

        if (user.Password != authDto.Password)
        {
            throw new UserAccessDeniedExceptions($"Email: {authDto.Email}, password: {authDto.Password}");
        }
        var claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier, user.Id.ToString()),
            new (ClaimTypes.Role, user.Role),
            new (ClaimTypes.Email, user.Email)
        };
        var now = DateTime.Now;
        var jwt = new JwtSecurityToken(
            issuer:AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            notBefore: now,
            claims: claims,
            expires: now.Add(TimeSpan.FromHours(AuthOptions.LIFETIME)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
        return encodedJwt;
    }
}