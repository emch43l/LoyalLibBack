namespace LoyalLib.Exception;

public class PasswordException : ExceptionCore
{
    public PasswordException(string message = "Wrong password !") : base(message)
    {
        
    }
}