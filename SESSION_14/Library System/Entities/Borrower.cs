using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Library_System.Entities
{
    internal class Borrower
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public DateOnly MembershipDate { get; set; } = DateOnly.Parse("09/09/9999");
        
        public ICollection<Book> BorrowedBooks { get; set; }
    
        public Borrower() 
        { 
            BorrowedBooks = new List<Book>();
        }
        public Borrower(string name, DateOnly membershipDate)
        {
            Name = name;
            MembershipDate = membershipDate;
            BorrowedBooks = new List<Book>();
        }
    }
}
