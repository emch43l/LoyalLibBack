using System.Security.Claims;
using LoyalLib.Db;
using LoyalLib.Entities;
using LoyalLib.Exception;
using Microsoft.AspNetCore.Identity;

namespace LoyalLib.Services.Identity;

public class IdentityService : IIdentityService
{
    private readonly DatabaseContext _context;
    
    private readonly UserManager<UserEntity> _manager;

    public IdentityService(UserManager<UserEntity> manager, DatabaseContext context)
    {
        _context = context;
        _manager = manager;
    }
    public async Task<UserEntity?> GetUserByEmailAsync(string email)
    {
        UserEntity? user = await _manager.FindByEmailAsync(email);

        await LoadTokenForUser(user);

        return user;
    }

    public async Task<bool> CheckPasswordAsync(UserEntity user, string password)
    {
        return await _manager.CheckPasswordAsync((UserEntity)user, password);
    }

    public async Task<UserEntity> CreateUserAsync(string email, string username, string password)
    {
        UserEntity? user = await GetUserByEmailAsync(email);
        if (user != null)
        {
            throw new UserAlreadyExistException($"User with this email already exist !");
        }

        UserEntity newUser = new UserEntity() { Email = email, UserName = username};
        IdentityResult result = await _manager.CreateAsync(newUser, password);
        if (!result.Succeeded)
        {
            throw new IdentityException(string.Join(',',result.Errors.Select(e => e.Description)));
        }

        return newUser;
    }

    public async Task<IdentityResult> UpdateUserRefreshTokenAsync(UserEntity user, RefreshTokenEntity token)
    {
        if (user.RefreshToken == null)
        {
            user.RefreshToken = token;
        }
        else
        {
            user.RefreshToken.TokenString = token.TokenString;
            user.RefreshToken.ExpireDate = token.ExpireDate;
        }
        
        return await _manager.UpdateAsync(user);
    }

    public async Task<UserEntity> GetUserByClaimAsync(ClaimsPrincipal claimsPrincipal)
    {
        UserEntity? user = await _manager.GetUserAsync(claimsPrincipal);
        if (user == null)
            throw new UserNotFoundException();

        await LoadTokenForUser(user);

        return user;
    }

    public async Task<IList<string>> GetUserRolesByEmailAsync(string email)
    {
        UserEntity? user = await GetUserByEmailAsync(email);
        if (user == null)
        {
            throw new UserNotFoundException();
        }
        
        IList<string> userRoles = await _manager.GetRolesAsync((UserEntity)user);
        return userRoles;
    }

    private async Task LoadTokenForUser(UserEntity? user)
    {
        if(user == null)
            return;
        await _context.Users.Entry(user).Reference(u => u.RefreshToken).LoadAsync();
    }

}