namespace LoyalLib.Exception;

public class UserAlreadyExistException : ExceptionCore
{
    public UserAlreadyExistException(string message = "User already exist !") : base(message)
    {
        
    }
}