using Microsoft.EntityFrameworkCore;

namespace BookShop.Data;

public class EntityContext : DbContext
{
    public EntityContext(DbContextOptions<EntityContext> options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Book> Books { get; set; }
}