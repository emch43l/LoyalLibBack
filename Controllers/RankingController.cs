using LoyalLib.DTO.Response;
using LoyalLib.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoyalLib.Controllers;

[Authorize]
[Route("ranking")]
public class RankingController : ControllerBase
{
    private readonly IRankingService _rankingService;

    public RankingController(IRankingService rankingService)
    {
        _rankingService = rankingService;
    }

    [HttpGet]
    public async Task<IActionResult> GetRankings()
    {
        IEnumerable<RankingResponse> response = await _rankingService.GetRanking();
        return Ok(response);
    }
}