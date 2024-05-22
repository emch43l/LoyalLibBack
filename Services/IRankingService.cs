using LoyalLib.DTO.Response;

namespace LoyalLib.Services;

public interface IRankingService
{
    public Task<IEnumerable<RankingResponse>> GetRanking();
}