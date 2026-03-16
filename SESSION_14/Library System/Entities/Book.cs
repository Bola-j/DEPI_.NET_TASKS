using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Library_System.Entities
{
    internal class Book
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string ISBN { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public ICollection<Borrower> Borrowers { get; set; }

        public Book(string title, string isbn, Author author)
        {
            Title = title;
            ISBN = isbn;
            Author = author;
            Borrowers = new List<Borrower>();
        }
        public Book()
        {
            Borrowers = new List<Borrower>();
        }

        public override string ToString()
        {
            return $"Title: {Title}, ISBN: {ISBN}, Author: {Author?.Name}, Borrowers: {Borrowers.Count} \n\t {string.Join("\n\t\t ", Borrowers)}";
        }
    }
}
