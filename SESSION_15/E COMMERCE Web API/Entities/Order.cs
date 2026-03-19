using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_COMMERCE_Web_API.Entities
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime OrderDate { get; set; }

        [Required]
        [ForeignKey(nameof(Customer))]
        public int CustomerId { get; set; }

        // Navigation made nullable to allow EF to materialize without eager loading
        public Customer? Customer { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }

        public Order()
        {
            OrderDetails = new List<OrderDetail>();
            OrderDate = DateTime.UtcNow;
        }

        public Order(Customer customer)
        {
            Customer = customer ?? throw new ArgumentNullException(nameof(customer));
            CustomerId = customer.Id;
            OrderDate = DateTime.UtcNow;
            OrderDetails = new List<OrderDetail>();
        }

        public override string ToString()
        {
            return $"Id: {Id}, OrderDate: {OrderDate}, CustomerId: {CustomerId}";
        }
    }
}
