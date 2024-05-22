using LoyalLib.DTO;

namespace LoyalLib.Services.Auth;

public interface IAuthService
{
    Task<AuthResult> Login(string email, string password);
    Task<AuthResult> Register(string email, string username, string password);
    Task<AuthResult> RefreshToken(string token, string refreshToken);
}