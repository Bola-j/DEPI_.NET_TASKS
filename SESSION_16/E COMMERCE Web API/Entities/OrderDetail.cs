using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_COMMERCE_Web_API.Entities
{
    public class OrderDetail
    {

        [Required]
        [ForeignKey(nameof(Order))]
        public int OrderId { get; set; }

        // Navigation made nullable to allow EF to materialize without eager loading
        public Order? Order { get; set; }

        [Required]
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }

        // Navigation made nullable to allow EF to materialize without eager loading
        public Product? Product { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        public OrderDetail() { }

        public OrderDetail(Order order, Product product, int quantity)
        {
            Order = order ?? throw new ArgumentNullException(nameof(order));
            OrderId = order.Id;
            Product = product ?? throw new ArgumentNullException(nameof(product));
            ProductId = product.Id;
            Quantity = quantity > 0 ? quantity : throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be at least 1.");
        }

        public override string ToString()
        {
            return $"OrderId: {OrderId}, ProductId: {ProductId}, Quantity: {Quantity}, Product: {Product?.Name}, OrderDate: {Order?.OrderDate}";
        }
    }
}