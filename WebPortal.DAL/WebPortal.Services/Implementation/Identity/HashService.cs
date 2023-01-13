using Services.Interfaces.Identity;

namespace Services.Implementation.Identity;

public class HashService : IHashService
{
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
    public bool VerifyPassword(string password, string hash)
    {
        var isVerify = BCrypt.Net.BCrypt.Verify(password, hash);
        return isVerify ? isVerify : throw new ArgumentException("Incorrect password!");
    }
}