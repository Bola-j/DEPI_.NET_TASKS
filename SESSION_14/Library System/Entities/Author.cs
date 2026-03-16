using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Library_System.Entities
{
    internal class Author
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public DateOnly BirthDate { get; set; } = DateOnly.Parse("09/09/9999");

        public ICollection<Book> Books { get; set; }
        public Author(string name, DateOnly birthDate)
        {
            Name = name;
            BirthDate = birthDate;
            Books = new List<Book>();
        }
        public Author()
        {
            Books = new List<Book>();
        }

        public override string ToString()
        {
            return $"Author: {Name}, Birth Date: {BirthDate}, Books: {Books.Count} \n\t {string.Join("\n\t\t ", Books)}";
        }
    }
}
