namespace LoyalLib.Exception;

public class UserNotFoundException : ExceptionCore
{
    public UserNotFoundException(string message = "User not found !") : base(message)
    {
        
    }
}