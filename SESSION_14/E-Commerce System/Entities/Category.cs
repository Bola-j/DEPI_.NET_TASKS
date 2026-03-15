using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce_System.Entities
{
    internal class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }

        public override string ToString()
        {
            return $"Id:{Id}, Category: {Name}";
        }
    }
}
