namespace ManagementBook.Infra.Data.Features.Books;

using ManagementBook.Domain.Books;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class BookEntityConfiguration : IEntityTypeConfiguration<Book>
{
    const string TABLE_NAME = "Books";
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable(TABLE_NAME);
        builder.Property(b => b.Title).IsRequired();
        builder.Property(b => b.Author).IsRequired();
        builder.Property(b => b.ReleaseDate).IsRequired();
        builder.Property(b => b.BookCoverUrl);
    }
}
