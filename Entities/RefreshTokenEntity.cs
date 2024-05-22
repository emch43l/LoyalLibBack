namespace LoyalLib.Entities;

public class RefreshTokenEntity
{
    public int Id { get; set; }
    
    public string TokenString { get; set; }
    
    public DateTime ExpireDate { get; set; }
    
    public int UserId { get; set; }

    public UserEntity User { get; set; }
}