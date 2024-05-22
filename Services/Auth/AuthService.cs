using System.Security.Claims;
using LoyalLib.DTO;
using LoyalLib.Entities;
using LoyalLib.Exception;
using LoyalLib.Services.Identity;
using LoyalLib.Services.Token;

namespace LoyalLib.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IIdentityService _identityService;
    private readonly ITokenService _tokenService;

    public AuthService(IIdentityService identityService, ITokenService tokenService)
    {
        _identityService = identityService;
        _tokenService = tokenService;
    }

    public async Task<AuthResult> Login(string email, string password)
    {
        UserEntity? user = await _identityService.GetUserByEmailAsync(email);

        if (user == null)
            throw new UserNotFoundException();

        bool isPasswordOk = await _identityService.CheckPasswordAsync(user, password);
        if (!isPasswordOk)
            throw new PasswordException();
        
        IList<string> roles = await _identityService.GetUserRolesByEmailAsync(user.Email!); 
        string token = _tokenService.CreateToken(user,roles);

        if (user.RefreshToken == null || user.RefreshToken?.ExpireDate < DateTime.Now)
        {
            RefreshTokenEntity refreshToken = _tokenService.CreateRefreshTokenForUser();
            await _identityService.UpdateUserRefreshTokenAsync(user, refreshToken);
        }

        return new AuthResult(token,user.RefreshToken!.TokenString,user.Id);
    }

    public async Task<AuthResult> Register(string email, string username, string password)
    {
        UserEntity user = await _identityService.CreateUserAsync(email, username, password);
        string token = _tokenService.CreateToken(user,new List<string>());
        RefreshTokenEntity refreshToken = _tokenService.CreateRefreshTokenForUser();
        await _identityService.UpdateUserRefreshTokenAsync(user, refreshToken);
        
        return new AuthResult(token, refreshToken.TokenString, user.Id);
    }

    public async Task<AuthResult> RefreshToken(string token, string refreshToken)
    {
        ClaimsPrincipal principal = await _tokenService.GetPrincipalFromToken(token);
        UserEntity? user = await _identityService.GetUserByClaimAsync(principal);
        
        if (user == null)
            throw new UserNotFoundException();

        if (user.RefreshToken?.TokenString == refreshToken)
        {
            IList<string> roles = await _identityService.GetUserRolesByEmailAsync(user.Email!);
            string newToken = _tokenService.CreateToken(user,roles);
            
            return new AuthResult(newToken, user.RefreshToken.TokenString, user.Id);
        }

        throw new TokenValidationException();

    }
}