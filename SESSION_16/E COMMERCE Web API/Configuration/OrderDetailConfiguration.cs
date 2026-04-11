using E_COMMERCE_Web_API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_COMMERCE_Web_API.Configuration
{
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            // Composite primary key
            builder.HasKey(od => new { od.OrderId, od.ProductId });

            builder.Property(od => od.Quantity)
                   .IsRequired();

            builder.HasOne(od => od.Order)
                   .WithMany(o => o.OrderDetails)
                   .HasForeignKey(od => od.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(od => od.Product)
                   .WithMany(p => p.OrderDetails)
                   .HasForeignKey(od => od.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
