namespace LoyalLib.DTO.Response;

public class ChallengeBookResponse
{
    public int Id { get; set; }

    public string Title { get; set; }
    
    public string Author { get; set; }
    
    public string Url { get; set; }
    
    public bool IsCompleted { get; set; }
}