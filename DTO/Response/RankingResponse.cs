namespace LoyalLib.DTO.Response;

public class RankingResponse
{
    public string UserName { get; set; }
    
    public int CompletedChallengesCount { get; set; }
    
    public int ReadBooksCount { get; set; }
    
    public int PointsCount { get; set; }
}