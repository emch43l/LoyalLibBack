using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace LoyalLib.Services.Token.Configuration;

public static class TokenConfiguration
{
    public const string SecionName = "JwtSettings";
    public const string KeySectionName = "Key";
    public const string IssuerSectionName = "Issuer";
    public const string AudienceSectionName = "Audience";
    public static TokenValidationParameters GetTokenValidationParameters(IConfiguration configuration)
    {
        IConfigurationSection jwtSection = configuration.GetSection(SecionName);
        
        string key = jwtSection.GetSection(KeySectionName).Value ?? throw new ArgumentNullException();
        string audience = jwtSection.GetSection(AudienceSectionName).Value ?? throw new ArgumentNullException();
        string issuer = jwtSection.GetSection(IssuerSectionName).Value ?? throw new ArgumentNullException();
        
        TokenValidationParameters parameters = new TokenValidationParameters();

        parameters.ClockSkew = TimeSpan.FromSeconds(0);
        parameters.SaveSigninToken = true;
        parameters.ValidateIssuer = true;
        parameters.ValidateAudience = true;
        parameters.ValidateIssuerSigningKey = true;
        parameters.ValidIssuer = issuer;
        parameters.ValidAudience = audience;
        parameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        
        
        return parameters;
    }
}