using BookShop.Data;

namespace BookShop.Repositories;

public class UserRepository
{
    private readonly EntityContext _context;

    public UserRepository(EntityContext context)
    {
        _context = context;
    }

    public User? GetById(Guid id)
    {
        var res = _context.Users
            .FirstOrDefault(u => u.Id == id);

        return res;
    }
    
    public User? GetByEmail(string email)
    {
        var res = _context.Users
            .FirstOrDefault(u => u.Email == email);

        return res;
    }

    public void Create(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
    }
}