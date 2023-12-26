namespace ManagementBook.Infra.Data.Base;

using ManagementBook.Domain.Books;
using ManagementBook.Infra.Data.Features.Books;
using Microsoft.EntityFrameworkCore;

public class BookStoreContext : DbContext
{
    public virtual DbSet<Book> Books { get; set; }

    public BookStoreContext(DbContextOptions<BookStoreContext> configuration) : base(configuration)
    {
        Database?.Migrate();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BookEntityConfiguration());
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    public virtual IQueryable<T> AsNoTracking<T>(IQueryable<T> query) where T : class
        => EntityFrameworkQueryableExtensions.AsNoTracking<T>(query);
}
