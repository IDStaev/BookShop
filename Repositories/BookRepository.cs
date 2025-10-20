using BookShop.Data;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Repositories;

public class BookRepository
{
    private readonly EntityContext _context;

    public BookRepository(EntityContext context)
    {
        _context = context;
    }

    public void Create(Book book)
    {
        _context.Books.Add(book);
        _context.SaveChanges();
    }

    public List<Book> GetAllFromUser(Guid id)
    {
        var res = _context.Books
            .Where(b => b.UserId == id)
            .ToList();

        return res;
    }

    public List<Book> GetPublic()
    {
        var res = _context.Books
            .Include(b=>b.User)
            .Where(b => b.IsPublic)
            .ToList();

        return res;
    }

    public Book? GetById(Guid id)
    {
        var res = _context.Books
            .FirstOrDefault(b => b.Id == id);

        return res;
    }

    public void Update(Book book)
    {
        _context.Books.Update(book);
        _context.SaveChanges();
    }

    public void Delete(Book book)
    {
        _context.Books.Remove(book);
        _context.SaveChanges();
    }
}