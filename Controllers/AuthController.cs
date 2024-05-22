using LoyalLib.DTO;
using LoyalLib.Entities;
using LoyalLib.Exception;
using LoyalLib.Services.Auth;
using LoyalLib.Services.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RefreshRequest = LoyalLib.DTO.Request.RefreshRequest;
using LoginRequest = LoyalLib.DTO.Request.LoginRequest;
using RegisterRequest = LoyalLib.DTO.Request.RegisterRequest;

namespace LoyalLib.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IIdentityService _identityService;
    public AuthController(IAuthService authService, IIdentityService identityService)
    {
        _authService = authService;
        _identityService = identityService;
    }
    
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        AuthResult result = await _authService.Login(request.Email,request.Password);
        return Ok(result);
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        AuthResult result = await _authService.Register(request.Email, request.Username, request.Password);
        return Ok(result);
    }
    
    [HttpPost]
    [Route("refresh")]
    public async Task<IActionResult> Login([FromBody] RefreshRequest refreshRequest)
    {
        try
        {
            AuthResult result = await _authService.RefreshToken(refreshRequest.Token,refreshRequest.RefreshToken);
            return Ok(result);
        }
        catch (TokenValidationException e)
        {
            return Unauthorized();
        }
        
    }

    [Authorize]
    [HttpGet]
    [Route("user")]
    public async Task<IActionResult> GetUser()
    {
        UserEntity user = await _identityService.GetUserByClaimAsync(HttpContext.User);
        return Ok(
            new { Email = user.Email, Username = user.UserName }
            );
    }
}