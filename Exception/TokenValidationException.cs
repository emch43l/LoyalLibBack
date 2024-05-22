namespace LoyalLib.Exception;

public class TokenValidationException : ExceptionCore
{
    public TokenValidationException(string message = "Invalid token !") : base(message)
    {
        
    }
}