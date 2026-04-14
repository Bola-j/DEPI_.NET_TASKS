using Library_System_Web_API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library_System_Web_API.Configuration
{
    public class LoanConfiguration : IEntityTypeConfiguration<Loan>
    {
        public void Configure(EntityTypeBuilder<Loan> builder)
        {
            builder.HasKey(l => l.Id);

            builder.Property(l => l.LoanDate)
                   .IsRequired();

            builder.Property(l => l.ReturnDate)
                   .IsRequired(false);

            builder.HasIndex(l => l.BookId)
                   .HasDatabaseName("IX_Loans_BookId");

            builder.HasIndex(l => l.BorrowerId)
                   .HasDatabaseName("IX_Loans_BorrowerId");

            builder.HasOne(l => l.Book)
                   .WithMany(b => b.Loans)
                   .HasForeignKey(l => l.BookId);

            builder.HasOne(l => l.Borrower)
                   .WithMany(b => b.Loans)
                   .HasForeignKey(l => l.BorrowerId);
        }
    }
}
