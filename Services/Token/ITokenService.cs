using System.Security.Claims;
using LoyalLib.Entities;

namespace LoyalLib.Services.Token;

public interface ITokenService
{
    RefreshTokenEntity CreateRefreshTokenForUser();
    string CreateRefreshToken();
    string CreateToken(UserEntity user, IEnumerable<string> roles);
    
    Task<ClaimsPrincipal> GetPrincipalFromToken(string token);
}