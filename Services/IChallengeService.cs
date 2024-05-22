using System.Security.Claims;
using LoyalLib.DTO.Request;
using LoyalLib.DTO.Response;
using LoyalLib.Entities;

namespace LoyalLib.Services;

public interface IChallengeService
{
    public Task<IEnumerable<ChallengeResponse>> GetUserChallenges(ClaimsPrincipal principal);

    public Task MarkBookRead(ReadBookRequest request, ClaimsPrincipal principal);

    public Task GenerateChallenge(ClaimsPrincipal principal);
}