using Library_System_Web_API.DTOs.Book;

namespace Library_System_Web_API.DTOs.Loan
{
    public class LoanWithBookDTO 
    {
        public SlimBookDTO? Book { get; set; }
        public DateOnly? LoanDate { get; set; }
        public DateOnly? ReturnDate { get; set; }
    }
}
