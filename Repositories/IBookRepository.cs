using LoyalLib.Entities;

namespace LoyalLib.Repositories;

public interface IBookRepository
{
    Task<BookEntity?> GetBookById(int id);

    Task<IEnumerable<BookEntity>> GetAllBooks();

    Task RemoveBook(BookEntity book);

    Task UpdateBook(BookEntity book);

    Task<BookEntity> CreateBook(BookEntity book);

    IQueryable<BookEntity> GetQuery();

    Task SaveChanges();
}