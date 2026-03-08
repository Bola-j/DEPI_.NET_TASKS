using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SESSION_12
{

    class Product : IComparable<Product>
    {
        public long ProductID { get; set; }
        public string ProductName { get; set; } 
        public string Category { get; set; }
        public decimal UnitPrice { get; set; }
        public int UnitsInStock { get; set; }

        public int CompareTo(Product? other)
            => this.UnitPrice.CompareTo(other?.UnitPrice);


        public override string ToString()
            => $"ProductID:{ProductID},ProductName:{ProductName},Category{Category},UnitPrice:{UnitPrice},UnitsInStock:{UnitsInStock}";

    }
    
    
    
}

