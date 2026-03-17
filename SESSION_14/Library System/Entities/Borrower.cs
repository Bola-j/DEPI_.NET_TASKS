using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library_System.Entities
{
    internal class Borrower
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateOnly MembershipDate { get; set; }

        public ICollection<Book> BorrowedBooks { get; set; }

        public Borrower()
        {
            BorrowedBooks = new List<Book>();
        }

        public Borrower(string name, DateOnly membershipDate)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            MembershipDate = membershipDate;
            BorrowedBooks = new List<Book>();
        }

        public override string ToString()
        {
            return $"Id: {Id}, Borrower: {Name}, Membership Date: {MembershipDate}";
        }
    }
}