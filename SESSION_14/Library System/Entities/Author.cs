using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library_System.Entities
{
    internal class Author
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateOnly BirthDate { get; set; }

        public ICollection<Book> Books { get; set; }

        public Author()
        {
            Books = new List<Book>();
        }

        public Author(string name, DateOnly birthDate)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            BirthDate = birthDate;
            Books = new List<Book>();
        }

        public override string ToString()
        {
            return $"Author: {Name}, Birth Date: {BirthDate}, Books: {Books.Count} \n\t {string.Join("\n\t\t ", Books)}";
        }
    }
}