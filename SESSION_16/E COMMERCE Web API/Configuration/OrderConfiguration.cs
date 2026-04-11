using E_COMMERCE_Web_API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_COMMERCE_Web_API.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.OrderDate)
                   .IsRequired();

            builder.HasIndex(o => o.CustomerId)
                   .HasDatabaseName("IX_Orders_CustomerId");

            builder.HasOne(o => o.Customer)
                   .WithMany(c => c.Orders)
                   .HasForeignKey(o => o.CustomerId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(o => o.OrderDetails)
                   .WithOne(od => od.Order)
                   .HasForeignKey(od => od.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
