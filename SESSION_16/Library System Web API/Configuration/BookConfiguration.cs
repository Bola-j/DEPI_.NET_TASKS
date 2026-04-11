using Library_System_Web_API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library_System_Web_API.Configuration
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Title)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(b => b.ISBN)
                   .IsRequired()
                   .HasMaxLength(13);

            builder.HasIndex(b => b.ISBN)
                   .IsUnique()
                   .HasDatabaseName("UX_Books_ISBN");

            builder.HasIndex(b => b.AuthorId)
                   .HasDatabaseName("IX_Books_AuthorId");

            builder.HasOne(b => b.Author)
                   .WithMany(a => a.Books)
                   .HasForeignKey(b => b.AuthorId);

            builder.HasMany(b => b.Loans)
                   .WithOne(l => l.Book)
                   .HasForeignKey(l => l.BookId);
        }
    }
}
