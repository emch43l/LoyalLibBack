using System.Security.Claims;
using System.Security.Cryptography;
using LoyalLib.Entities;
using LoyalLib.Exception;
using LoyalLib.Services.Identity;
using LoyalLib.Services.Token.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace LoyalLib.Services.Token;

public class TokenService : ITokenService
{
    private const int TokenLifetime = 120;
    
    private const string TokenAlgorithm = SecurityAlgorithms.HmacSha256;

    private readonly IIdentityService _identityService;

    private readonly IConfiguration _configuration;

    private readonly TokenValidationParameters _parameters;

    public TokenService(IIdentityService identityService, IConfiguration configuration)
    {
        _identityService = identityService;
        _configuration = configuration;
        _parameters = TokenConfiguration.GetTokenValidationParameters(_configuration);
    }

    public RefreshTokenEntity CreateRefreshTokenForUser()
    {
        RefreshTokenEntity token = new RefreshTokenEntity() 
        {
            TokenString = CreateRefreshToken(),
            ExpireDate = DateTime.Now.AddDays(7)
        };

        return token;
    }

    public string CreateToken(UserEntity user, IEnumerable<string> roles)
    {
        
        SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor();
        JsonWebTokenHandler handler = new JsonWebTokenHandler();
        
        handler.SetDefaultTimesOnTokenCreation = false;

        Dictionary<string, object> tokenClaims = new Dictionary<string, object>();
        
        tokenClaims.Add(ClaimTypes.NameIdentifier,user.Id);
        tokenClaims.Add(JwtRegisteredClaimNames.Exp, DateTimeOffset.UtcNow.AddMinutes(TokenLifetime).ToUnixTimeMilliseconds());
        tokenClaims.Add(ClaimTypes.Role, roles);
        
        descriptor.Claims = tokenClaims;
        descriptor.NotBefore = DateTime.Now;
        descriptor.Expires = DateTime.UtcNow.AddMinutes(10);
        //descriptor.Expires = DateTime.UtcNow.AddMinutes(TokenLifetime);
        descriptor.Audience = _parameters.ValidAudience;
        descriptor.Issuer = _parameters.ValidIssuer;
        descriptor.SigningCredentials = new SigningCredentials(_parameters.IssuerSigningKey,TokenAlgorithm);
        
        
        string token = handler.CreateToken(descriptor);
        return token;
    }

    public async Task<ClaimsPrincipal> GetPrincipalFromToken(string token)
    {
        JsonWebTokenHandler handler = new JsonWebTokenHandler();
        TokenValidationParameters parameters = _parameters;
        parameters.ValidateLifetime = false;
        
        TokenValidationResult result = await handler.ValidateTokenAsync(token, parameters);
        if (!result.IsValid)
        {
            throw new TokenValidationException(result.Exception.Message);
        }

        return new ClaimsPrincipal(new ClaimsIdentity(result.ClaimsIdentity));
    }

    public string CreateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}