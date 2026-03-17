using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library_System.Entities
{
    internal class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Title { get; set; }

        [Required]
        [StringLength(13, MinimumLength = 10)]
        [RegularExpression(@"^(?:\d{10}|\d{13})$", ErrorMessage = "ISBN must be 10 or 13 digits.")]
        public string ISBN { get; set; }

        [Required]
        [ForeignKey(nameof(Author))]
        public int AuthorId { get; set; }

        [Required]
        public Author Author { get; set; }

        public ICollection<Borrower> Borrowers { get; set; }

        public Book()
        {
            Borrowers = new List<Borrower>();
        }

        public Book(string title, string isbn, Author author)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            ISBN = isbn ?? throw new ArgumentNullException(nameof(isbn));
            Author = author ?? throw new ArgumentNullException(nameof(author));
            AuthorId = author.Id;
            Borrowers = new List<Borrower>();
        }

        public override string ToString()
        {
            return $"Title: {Title}, ISBN: {ISBN}, Author: {Author?.Name}, Borrowers: {Borrowers.Count} \n\t {string.Join("\n\t\t ", Borrowers)}";
        }
    }
}