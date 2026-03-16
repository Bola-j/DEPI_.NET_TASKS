using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_System.Entities
{
    internal class Loan
    {

        public int BookId { get; set; }
        public Book Book { get; set; }
        public int BorrowerId { get; set; }
        public Borrower Borrower { get; set; }
        public DateOnly LoanDate { get; set; } = DateOnly.Parse("09/09/9999");
        public DateOnly ReturnDate { get; set; } = DateOnly.Parse("09/09/9999");
    }
}
