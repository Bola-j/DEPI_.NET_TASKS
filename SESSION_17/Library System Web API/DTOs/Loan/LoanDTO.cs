using System.Text.Json.Serialization;
using Library_System_Web_API.DTOs.Borrower;
using Library_System_Web_API.DTOs.Book;
using System.Text.Json.Serialization;
namespace Library_System_Web_API.DTOs.Loan
{
    public class LoanDTO : SlimLoanDTO
    {
        [JsonPropertyName("borrower")]
        [JsonPropertyOrder(5)]
        public SlimBorrowerDTO? Borrower { get; set; }
        [JsonPropertyName("book")]
        [JsonPropertyOrder(6)]
        public SlimBookDTO? Book { get; set; }
    }
}
