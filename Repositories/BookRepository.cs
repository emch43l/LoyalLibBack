using LoyalLib.Db;
using LoyalLib.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoyalLib.Repositories;

public class BookRepository : IBookRepository
{
    private readonly DatabaseContext _context;

    public BookRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<BookEntity?> GetBookById(int id)
    {
        return await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<IEnumerable<BookEntity>> GetAllBooks()
    {
        return await _context.Books.ToListAsync();
    }

    public async Task RemoveBook(BookEntity book)
    {
        await Task.FromResult(_context.Books.Remove(book));
    }

    public async Task UpdateBook(BookEntity book)
    {
        await Task.FromResult(_context.Books.Update(book));
    }

    public async Task<BookEntity> CreateBook(BookEntity book)
    {
        return (await _context.Books.AddAsync(book)).Entity;
    }

    public IQueryable<BookEntity> GetQuery()
    {
        return _context.Books;
    }

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }
}