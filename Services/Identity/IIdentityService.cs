using System.Security.Claims;
using LoyalLib.Entities;
using Microsoft.AspNetCore.Identity;

namespace LoyalLib.Services.Identity;

public interface IIdentityService
{
    Task<UserEntity> GetUserByClaimAsync(ClaimsPrincipal claimsPrincipal);
    
    Task<IList<string>> GetUserRolesByEmailAsync(string email);
    
    Task<UserEntity?> GetUserByEmailAsync(string email);
    
    Task<bool> CheckPasswordAsync(UserEntity user, string password);

    Task<UserEntity> CreateUserAsync(string email, string username,  string password);

    Task<IdentityResult> UpdateUserRefreshTokenAsync(UserEntity user, RefreshTokenEntity token);

}