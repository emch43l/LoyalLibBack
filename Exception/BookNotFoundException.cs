namespace LoyalLib.Exception;

public class BookNotFoundException : ExceptionCore
{
    public BookNotFoundException(string message = "Book not found !") : base(message)
    {
        
    }
}