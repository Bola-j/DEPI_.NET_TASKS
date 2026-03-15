using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce_System.Entities
{
    internal class OrderDetail
    {
        
        public int OrderId { get; set; }

        public Order Order { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        public int Quantity { get; set; }

        public override string ToString()
        {
            return $"OrderId: {OrderId}, ProductId: {ProductId}, Quantity: {Quantity}, Product: {Product?.Name}, OrderDate: {Order?.OrderDate}";
        }
    }
}
