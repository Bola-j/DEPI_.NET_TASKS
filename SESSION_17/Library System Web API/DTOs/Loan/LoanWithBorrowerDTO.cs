using Library_System_Web_API.DTOs.Borrower;
using System;
using System.Text.Json.Serialization;
namespace Library_System_Web_API.DTOs.Loan
{
    public class LoanWithBorrowerDTO 
    {
        public SlimBorrowerDTO? Borrower { get; set; }
        public DateOnly? LoanDate { get; set; }
        public DateOnly? ReturnDate { get; set; }
    }
}
