using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce_System.Entities
{
    internal class Order
    {
        [Key]
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; }

        public Customer Customer { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, OrderDate: {OrderDate}, Customer: {Customer?.Name}";
        }
    }
}
