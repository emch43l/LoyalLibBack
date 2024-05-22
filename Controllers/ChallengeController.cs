using LoyalLib.DTO.Request;
using LoyalLib.DTO.Response;
using LoyalLib.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoyalLib.Controllers;

[ApiController]
[Authorize]
[Route("challenges")]
public class ChallengeController : ControllerBase
{
    private readonly IChallengeService _challengeService;

    public ChallengeController(IChallengeService challengeService)
    {
        _challengeService = challengeService;
    }

    [HttpGet]
    public async Task<IActionResult> GetUserChalleges()
    {
        IEnumerable<ChallengeResponse> challengeResponse = await _challengeService.GetUserChallenges(HttpContext.User);
        return Ok(challengeResponse);
    }

    [HttpPost]
    public async Task<IActionResult> MarkBookRead([FromBody] ReadBookRequest request)
    {
        await _challengeService.MarkBookRead(request, HttpContext.User);
        return Ok();
    }

    [HttpPost]
    [Route("generate")]
    public async Task<IActionResult> GenerateChallenge()
    {
        await _challengeService.GenerateChallenge(HttpContext.User);
        return Ok();
    }
    
}