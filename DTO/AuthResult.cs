namespace LoyalLib.DTO;

public record AuthResult(string Token, string RefreshToken, int UserId);