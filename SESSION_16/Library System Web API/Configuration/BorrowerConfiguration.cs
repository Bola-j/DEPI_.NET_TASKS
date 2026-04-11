using Library_System_Web_API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library_System_Web_API.Configuration
{
    public class BorrowerConfiguration : IEntityTypeConfiguration<Borrower>
    {
        public void Configure(EntityTypeBuilder<Borrower> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(b => b.MembershipDate)
                   .IsRequired();

            builder.HasMany(b => b.Loans)
                   .WithOne(l => l.Borrower)
                   .HasForeignKey(l => l.BorrowerId);
        }
    }
}
