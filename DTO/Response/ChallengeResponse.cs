namespace LoyalLib.DTO.Response;

public class ChallengeResponse
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public int Points { get; set; }
    public ICollection<ChallengeBookResponse> Books { get; set; }

    public int BooksCount => Books.Count;

    public int CompletedBooksCount => Books.Count(b => b.IsCompleted == true);

    public bool IsCompleted => Books.Count(b => b.IsCompleted == true) == Books.Count();
}