using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce_System.Entities
{
    internal class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }

        [Required]
        public Category Category { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }

        public Product()
        {
            OrderDetails = new List<OrderDetail>();
        }

        public Product(string name, decimal price, Category category)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Price = price > 0 ? price : throw new ArgumentOutOfRangeException(nameof(price), "Price must be greater than 0.");
            Category = category ?? throw new ArgumentNullException(nameof(category));
            CategoryId = category.Id;
            OrderDetails = new List<OrderDetail>();
        }

        public override string ToString()
        {
            return $"Id: {Id}, Product: {Name}, Price: {Price:C}, Category: {Category?.Name}";
        }
    }
}