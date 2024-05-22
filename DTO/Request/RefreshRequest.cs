namespace LoyalLib.DTO.Request;

public class RefreshRequest
{
    public required string RefreshToken { get; set; }
    
    public required string Token { get; set; }
}