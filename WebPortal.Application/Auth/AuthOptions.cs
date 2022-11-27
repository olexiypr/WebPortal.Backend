using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace WebPortal.Application.Auth;

public class AuthOptions
{
    public const string ISSUER = "WebPortal";
    public const string AUDIENCE = "WebPortal"; 
    const string KEY = "mysupersecret_secretkey!123";   
    public const int LIFETIME = 10;
    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
    }
}