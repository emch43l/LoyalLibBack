using System.ComponentModel.DataAnnotations.Schema;

namespace LoyalLib.Entities;

public class ChallengeEntity
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public int Points { get; set; }
    
    public int UserId { get; set; }

    public UserEntity User { get; set; } = null!;

    public ICollection<BookEntity> RequiredBooks { get; set; }
}